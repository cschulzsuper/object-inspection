using Microsoft.AspNetCore.Identity;
using System;

namespace Super.Paula.Application.Auth
{
    public class IdentityPasswordHasher : PasswordHasher<Identity>
    {
        public override PasswordVerificationResult VerifyHashedPassword(Identity user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword.Equals(providedPassword))
            {
                // We need to make sure that the provided password itself is not the hash of the hashed password

                var decodedHashedPassword = new Span<byte>(new byte[hashedPassword.Length]);
                var hashedPasswordIsBase64 = Convert.TryFromBase64String(hashedPassword, decodedHashedPassword, out _);

                var providedPasswordIsTheHash = hashedPasswordIsBase64 &&
                    (decodedHashedPassword[0] == 0x00 ||
                     decodedHashedPassword[0] == 0x01);

                // If the provied password is not the hash than it is the old plain text password
                // A rehash is necessary

                return providedPasswordIsTheHash
                    ? PasswordVerificationResult.Failed
                    : PasswordVerificationResult.SuccessRehashNeeded;
            }

            return base.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }
    }
}
