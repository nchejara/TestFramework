using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFramework.Framework;
namespace TestFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            AddGroup();
            Console.ReadLine();
        }

        public static async void AddGroup()
        {
            TestGroup group = new TestGroup("Test Group a");

            group.AddTestcase(Ip4Validation("1 - Null Validation", null));
            group.AddTestcase(Ip4Validation("2 - . segment validation", "..."));
            group.AddTestcase(Ip4Validation("3 - Lenght validation", "000.0000.000.0000"));
            group.AddTestcase(Ip4Validation("4 - contain char", "aa.0000.000.0000"));
            group.AddTestcase(Ip4Validation("5 - Check valid IP", "172.0.0.0"));

            await group.Run();
            foreach (string logs in group.GetLogs())
            {
                Console.WriteLine(logs);

            }

        }
        public static Testcase Ip4Validation(string testcasename, string str)
        {
            return new Testcase(testcasename, async delegate(Testcase testcase)
            {
                int i = isValidIp4(str);
                await Task.Delay(200);
                if (i == 0)
                    return false;
                return true;
            });
        }

        /// <summary>
        /// Validate IP4 address which is in form of string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int isValidIp4(string str)
        {
            if (str == null)
                return 0;

            int len = str.Length;
            if (len > 16) // string length should be lessthan or equal to 16
                return 0;
            
            //This validation based on the function requirement
            if (str[0] == '.') // first char shouldn't be '.'
                return 0;

            int segValue = 0; // use to count each segment value
            int charCount = 0; // char count in the segment
            int segCount = 1; // count total segment

            for (int i = 0; i < len; i++)
            {
                if (str[i] == '.')
                {
                    // SegCount should be 4 not more then that
                    if (segCount++ > 4)
                        return 0;

                    //charCount should be 3 not more than that
                    if (charCount > 3 || charCount == 0) // zero condition is based on function requirement
                        return 0;
                    
                    charCount = segValue = 0; // assign 0 for both charCount and segValue because we need to again count for other segment
                    continue;

                }

                //Return 0 if string consist below 0 and above 9 digit (it means return 0 if value is other than [0-9]
                if (str[i] < '0' || str[i] > '9')
                    return 0;

                //check max value of segment
                if((segValue = segValue * 10 + (str[i] - '0')) > 255)
                    return 0;

                charCount++;
            }

            if (segCount != 4)
                return 0;

            if (charCount > 3)
                return 0;


            return 1;
        }
        
  
    }
}
