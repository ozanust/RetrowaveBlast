#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("tm4qH4cto7Zse4twbhG6S7WPqIgK+pDd2R0TqbczzDDSLYZhCn2W/6YlKyQUpiUuJqYlJSThL5iEx5hxYrD5Vw7gH/mO0AHi8ZnQjoSiHocq/AzBRplP/KU9Z9r2Nc/xeQx92xSmJQYUKSItDqJsotMpJSUlISQnIHnedKoYnDttpnc36ZqcoNXdWDhQJBFg6PpYJBrKatBKfmK1UHFp6Y17Oefezbhbe5U/SoLANsgw2zlz1+tI1AVZAPlYi6MFNFXwb57ZqqAv/T7e3Xi/8Ac1LNReBpnmYKC9sDRkHpFu3dLeCJvSDsESjbwMS8/rsSExfpFtTOp0a4uyJxYjUCUQySddO5J6mRsHzh4n+LW2cNPxSPRlpau8RlOc4aZPMyYnJSQl");
        private static int[] order = new int[] { 6,5,5,7,12,6,7,12,8,10,13,13,12,13,14 };
        private static int key = 36;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
