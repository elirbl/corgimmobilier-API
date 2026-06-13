using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

/// <summary>Implémentation simulée : journalise l'e-mail au lieu de l'envoyer (à remplacer par un fournisseur SMTP réel).</summary>
public class EmailService(ILogger<EmailService> logger) : IEmailService
{
    public Task SendAsync(string to, string subject, string body)
    {
        logger.LogInformation("E-mail simulé envoyé à {To} | Sujet : {Subject}\n{Body}", to, subject, body);
        return Task.CompletedTask;
    }
}
