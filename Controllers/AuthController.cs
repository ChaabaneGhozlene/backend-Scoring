using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using scoring_Backend.Models.Admin;

namespace backend_scoring.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;
        private readonly SqrAdminContext _context; // ✅ Ajouter


    public AuthController(AuthService service, SqrAdminContext context) // ✅ Ajouter SqrAdminContext
    {
        _service = service;
        _context = context; 

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
       /* var result = await _service.Login(dto.Login, dto.Password);

        if (!result)
            return Unauthorized("Login ou mot de passe incorrect ou accès refusé");

        return Ok(new {
            message = "Connexion réussie",
            login = HttpContext.Session.GetString("userLogin"),
            role  = HttpContext.Session.GetInt32("userRole")
        });*/
        var token = await _service.Login(dto.Login, dto.Password);

        if (token == null)
            return Unauthorized("Login ou mot de passe incorrect ou accès refusé");

        return Ok(new {
            token     = token,
            expiresAt = DateTime.Now.AddHours(8)
        });
    }
    //  Endpoint temporaire pour hasher les mots de passe
    [HttpPost("hash-passwords")]
    
public async Task<IActionResult> HashAllPasswords()
{
    var users = await _context.Users.ToListAsync();
    int count = 0;

    foreach (var user in users)
    {
        if (string.IsNullOrWhiteSpace(user.Password))
            continue; // ignore les mots de passe vides

        // Ignore si déjà hashé correctement
        if (user.Password.StartsWith("$2a$") || user.Password.StartsWith("$2b$") || user.Password.StartsWith("$2y$"))
            continue;

        try
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            count++;
        }
        catch (BCrypt.Net.SaltParseException ex)
        {
            // Log pour debug mais ne bloque pas le processus
            Console.WriteLine($"⚠ Impossible de hasher l'utilisateur {user.Id}: {ex.Message}");
        }
    }

    await _context.SaveChangesAsync();
    return Ok($"{count} mots de passe hashés avec succès");
}

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok("Déconnecté");
    }
}