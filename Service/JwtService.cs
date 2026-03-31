using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using scoring_Backend.Models.Admin;

/// <summary>
/// Service responsable de la génération des tokens JWT.
/// </summary>
public class JwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Convertit le Role numérique en nom de rôle métier.
    ///
    /// Correspondance avec la table [SQR_Admin].[dbo].[UsersRoles] :
    ///   ID=1 → SuperAdmin  : voit tous les enregistrements
    ///   ID=2 → Admin       : voit uniquement son site (CustomerID)
    ///   ID=3 → Supervisor  : son site + campagnes + agents assignés
    ///   ID=4 → Agent       : ses propres enregistrements (AgentOid)
    ///   ID=5 → SuperUser   : voit tous les enregistrements (= SuperAdmin)
    ///
    /// Ces chaînes sont lues dans RecordRepository via le switch(userRole).
    /// Le switch utilise ToLowerInvariant() donc la casse n'a pas d'importance.
    /// </summary>
    public static string MapRole(int? roleId) => roleId switch
    {
        1 => "SuperAdmin",
        2 => "Admin",        // ← "Admin" correspond à la BDD (pas "AdminSite")
        3 => "Supervisor",   // ← "Supervisor" correspond à la BDD (pas "Superviseur")
        4 => "Agent",
        5 => "SuperUser",
        _ => "Agent"         // valeur par défaut sécurisée
    };

    /// <summary>
    /// Génère un token JWT pour un utilisateur authentifié.
    /// </summary>
    public string GenerateToken(User user)
    {
        var roleName = MapRole(user.Role);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name,           user.Login),
            new Claim(ClaimTypes.Role,           roleName),
            new Claim("userId",    user.Id.ToString()),
            new Claim("userLogin", user.Login),
            new Claim("userRole",  roleName),
        };

        var key   = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:             _config["Jwt:Issuer"],
            audience:           _config["Jwt:Audience"],
            claims:             claims,
            expires:            DateTime.Now.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}