using System.Security.Cryptography;

namespace api.Utils
{
    public class Salt
    {
        public byte[] getSalt(int saltLength)
        {
            byte[] randomNumber = new byte[saltLength];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return randomNumber;
            }
        }
    }
}
