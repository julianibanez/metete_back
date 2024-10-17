using MailKit.Security;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Constants;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Infraestructure.Smtp;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using StringRandomizer;
using System.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Metete.Api.Features.Auth.Commands
{
    public class RecoverPasswordUsuario
    {
        public class Command : IRequest<Unit>
        {
            public string Username { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly MeteteContext _context;
            private readonly MailSettings _mailSettings;


            public Handler(MeteteContext context, IOptions<MailSettings> mailSettings)
            {
                _context = context;
                _mailSettings = mailSettings.Value;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                Usuario? usuario = await _context.Usuarios
                   .FirstOrDefaultAsync(x => x.Username == command.Username, cancellationToken);

                if (usuario == null)
                {
                    throw new LoginFailedException(Feedback.USUARIO_NOT_FOUND);
                }

                var randomizer = new Randomizer();
                var recoveryCode = randomizer.Next();
                var expiryTime = DateTime.UtcNow.AddMinutes(30);

                var mail = new MimeMessage();

                // Sender / Receiver
                mail.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.From));
                mail.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.From);

                // Receiver
                mail.To.Add(MailboxAddress.Parse(usuario.Username));

                // Content

                StringBuilder body = new StringBuilder();

                body.Append($"<p>Hola {usuario.Nombre}</p>");                
                body.Append($"<p>Recibimos una solicitud para acceder a su cuenta de Metete {usuario.Username}. Su código de verificación de Metete es:</p>");
                body.Append($"<p><h1 style='text-align:center; font-weight:bold'>{recoveryCode}</h1></p>");
                body.Append($"<p>Si no solicitó este código, es posible que alguien más esté intentando acceder a la cuenta de Metete {usuario.Username}. No reenvíe ni proporcione este código a nadie.</p>");
                body.Append("<br />");
                body.Append("<p>Saludos,</p>");
                body.Append("<p>El equipo de cuentas de Metete</p>");

                var builder = new BodyBuilder();
                mail.Subject = "Código de verificación de Metete";
                builder.HtmlBody = body.ToString();
                mail.Body = builder.ToMessageBody();

                //  Send Mail
                using var smtp = new SmtpClient();

                if (_mailSettings.UseSSL)
                {
                    await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.SslOnConnect, cancellationToken);
                }
                else if (_mailSettings.UseStartTls)
                {
                    await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls, cancellationToken);
                }

                await smtp.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password, cancellationToken);
                await smtp.SendAsync(mail, cancellationToken);
                await smtp.DisconnectAsync(true, cancellationToken);

                usuario.RecoveryCode = recoveryCode;
                usuario.RecoveryCodeExpiryTime = expiryTime;

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
