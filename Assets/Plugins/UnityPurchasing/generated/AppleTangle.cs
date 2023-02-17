#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("4mwm47qtFvgQo4R50BbLX6qxqBGNkZjdr5KSid2+vM3j6vDNy83Jz5OZ3Z6Sk5mUiZSSk47dkpvdiI6YpFr49IHqvavs44kuSnbexrpeKJJK5kBuv9nv1zry4EuwYaOeNbZ96oTdnI6OiJCYjt2cnp6YjYmck56Y0s18Pvv11vv8+Pj6///NfEvnfE64g+Kxlq1rvHQ5iZ/27X68es53fImVko+UiYTM683p+/6o+f7u8LyNzsunzZ/M9s30+/6o+fvu/6iuzO6vmJGUnJOemN2Sk92JlZSO3Z6Yj0MJjmYTL5nyNoSyySVfwwSFApY13b68zX/8383w+/TXe7V7CvD8/PzdkpvdiZWY3YmVmJPdnI2NkZSenEjHUAny8/1v9kzc69OJKMHwJp/r9db7/Pj4+v/86+OViYmNjsfS0opVIYPfyDfYKCTyK5YpX9ne7ApcUWhjh/FZunamKevKzjY58rAz6ZQsPZ7OigrH+tGrFify3PMnR47kskiHzX/8i83z+/6o4PL8/AL5+f7//NvN2fv+qPn27uC8jY2RmN2+mI+JlJuUnpyJlJKT3byIiZWSj5SJhMzR3Z6Yj4mUm5SenImY3Y2SkZSehMDbmt13zpcK8H8yIxZe0gSul6aZy2Sx0IVKEHFmIQ6KZg+LL4rNsjyZyN7otuik4E5pCgthYzKtRzylrVZejG+6rqg8UtK8TgUGHo0wG16xNOSPCKDzKIKiZg/Y/keocrCg8AzyYMAO1rTV5zUDM0hE8ySj4Ss2wIK8VWUELDebYdmW7C1eRhnm1z7i+/6o4PP56/np1i2UummL9AMJlnDw+/TXe7V7CvD8/Pj4/f5//Pz9oeJ4fnjmZMC6yg9UZr1z0SlMbe8l9aPNf/zs+/6o4N35f/z1zX/8+c3Nf/lGzX/+Xl3+//z///z/zfD79MjPzMnNzsun6vDOyM3PzcTPzMnNn5GY3Y6JnJOZnI+Z3YmYj5CO3Zx//P379Nd7tXsKnpn4/M18D83X+83s+/6o+ffu97yNjZGY3bSTntPM+P3+f/zy/c1//Pf/f/z8/RlsVPTXe7V7CvD8/Pj4/c2fzPbN9Pv+qPoRgMR+dq7dLsU5TEJnsveWAtYBiZSblJ6ciZjdn4TdnJOE3Y2cj4ndnJOZ3Z6Yj4mUm5SenImUkpPdjY2RmN2+mI+JlJuUnpyJlJKT3byI683p+/6o+f7u8LyNjZGY3a+SkomacvVJ3Qo2UdHdko1LwvzNcUq+MtO9Wwq6sIL1o83i+/6o4N755c3rj5yeiZSemN2OiZyJmJCYk4mO082RmN20k57TzNvN2fv+qPn27uC8jdkfFixKjSLyuBzaNwyQhRAaSOrqtCWLYs7pmFyKaTTQ//78/fxef/x96dYtlLppi/QDCZZw071bCrqwgvn77v+orszuzez7/qj59+73vI2NTM2lEaf5z3GVTnLgI5iOApqjmEEky4I8eqgkWmREz78GJSiMY4Ncr/vN8vv+qODu/PwC+fjN/vz8As3gduR0IwS2kQj6Vt/N/xXlwwWt9C6KitOcjY2RmNOekpDSnI2NkZienHKOfJ075qb00m9PBbm1DZ3FY+gIrVd3KCcZAS30+spNiIjc");
        private static int[] order = new int[] { 15,6,43,52,59,40,37,25,40,40,38,50,13,41,23,35,54,18,57,28,38,50,44,56,48,38,48,28,44,57,30,47,49,33,35,38,42,38,40,56,40,51,56,45,59,51,50,47,50,52,53,59,52,59,59,58,57,59,59,59,60 };
        private static int key = 253;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
