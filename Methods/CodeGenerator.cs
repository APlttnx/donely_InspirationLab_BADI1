using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Methods
{
    public static class CodeGenerator
    {
        public static string Generate(int length = 8)
        {
            Char[] charArr  = "AZERTYUIOPLKJHGFDSQWXCVBN1234567890".ToCharArray();
            Random rng = new Random();
            string code = "";
            for (int i = 0; i < length; i++) 
                code += charArr[rng.Next(0, charArr.Length)];              
            Console.WriteLine(code);
            return code;
        }
    }
}
