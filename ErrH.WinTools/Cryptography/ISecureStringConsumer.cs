using System.Security;

namespace ErrH.WinTools.Cryptography
{
    public interface ISecureStringConsumer
    {
        void ReceiveKey(SecureString key);
    }
}
