using scoring_Backend.Repositories.Interfaces;

public class AuthService
{
    private readonly IAuthRepository _repository;
    private readonly JwtService      _jwtService;
    private readonly string          _codeApp;

    public AuthService(IAuthRepository repository, JwtService jwtService, IConfiguration config)
    {
        _repository = repository;
        _jwtService = jwtService;
        _codeApp    = config["codeApp"] ?? "";
        Console.WriteLine($"[CONFIG] codeApp lu: '{_codeApp}'");
    }

    public async Task<string?> Login(string login, string password)
    {
        // ÉTAPE 1 : Recherche utilisateur
        var userLocal = await _repository.GetUserByLogin(login);
        Console.WriteLine($"[STEP 1] User trouvé: {(userLocal != null ? "OUI" : "NON")}");

        if (userLocal != null)
        {
            Console.WriteLine($"[STEP 1] Role: {userLocal.Role}, IsActive: {userLocal.IsActive}");
            await _repository.SyncUserByRole(userLocal);
        }
        else
        {
            Console.WriteLine($"[STEP 2] Tentative SyncAddUser pour: {login}");
            await _repository.SyncAddUser(login);
            userLocal = await _repository.GetUserByLogin(login);
            Console.WriteLine($"[STEP 2] Après sync, user trouvé: {(userLocal != null ? "OUI" : "NON")}");
        }

        // ÉTAPE 3 : Vérification utilisateur
        if (userLocal == null)
        {
            Console.WriteLine("[STEP 3] ❌ User NULL");
            return null;
        }

        //  CORRECTION : vérifier si le hash est valide avant d'appeler BCrypt.Verify
        bool isValidBCryptHash = !string.IsNullOrWhiteSpace(userLocal.Password) && (
            userLocal.Password.StartsWith("$2a$") ||
            userLocal.Password.StartsWith("$2b$") ||
            userLocal.Password.StartsWith("$2y$")
        );

        Console.WriteLine($"[STEP 3] Hash BCrypt valide: {(isValidBCryptHash ? "OUI" : "NON")}");
        Console.WriteLine($"[STEP 3] Password en base (5 premiers chars): '{userLocal.Password?.Substring(0, Math.Min(5, userLocal.Password?.Length ?? 0))}'");

        if (!isValidBCryptHash)
        {
            // Mot de passe en texte clair → comparaison directe
            if (userLocal.Password != password)
            {
                Console.WriteLine("[STEP 3] ❌ Mot de passe incorrect (texte clair)");
                return null;
            }

            //  Migration automatique vers BCrypt
            Console.WriteLine("[STEP 3] ⚠ Migration du mot de passe en clair vers BCrypt...");
            userLocal.Password = BCrypt.Net.BCrypt.HashPassword(password);
            await _repository.UpdateUserPassword(userLocal);
            Console.WriteLine("[STEP 3]  Migration réussie");
        }
        else
        {
            // Mot de passe hashé → vérification BCrypt sécurisée
            try
            {
                bool isCorrect = BCrypt.Net.BCrypt.Verify(password, userLocal.Password);
                if (!isCorrect)
                {
                    Console.WriteLine("[STEP 3] ❌ Mot de passe incorrect (BCrypt)");
                    return null;
                }
            }
            catch (BCrypt.Net.SaltParseException ex)
            {
                //  Hash corrompu en base → refuser proprement sans crash
                Console.WriteLine($"[STEP 3]  Hash BCrypt corrompu en base: {ex.Message}");
                return null;
            }
        }

        // ÉTAPE 3b : Vérification compte actif
        if (userLocal.IsActive != 1)
        {
            Console.WriteLine($"[STEP 3b]  User inactif: IsActive={userLocal.IsActive}");
            return null;
        }

        // ÉTAPE 4 : Vérification accès application
        var hasAccess = await _repository.UserHasAccessToApp(userLocal.Id, _codeApp);
        Console.WriteLine($"[STEP 4] Accès '{_codeApp}': {(hasAccess ? "OUI" : "NON")}");

        if (!hasAccess)
        {
            Console.WriteLine("[STEP 4]  Accès refusé");
            return null;
        }

        // ÉTAPE 5 : Générer JWT
        var token = _jwtService.GenerateToken(userLocal);
        Console.WriteLine($"[STEP 5]  Token généré pour: {userLocal.Login}");

        return token;
    }
}