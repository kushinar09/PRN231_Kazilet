﻿using System;

namespace PRN231_Kazilet_API.Utils
{
    public class GameplayUtils
    {
        public static int GenerateRandom(int max)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, max); // Sinh ra một số có 6 chữ số
            return randomNumber;
        }
        public static string GenerateUniqueRandomNumbers()
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 1000000); // Sinh ra một số có 6 chữ số
            return randomNumber.ToString();
        }
    }
}
