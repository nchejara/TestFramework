using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.Framework
{
    public class TestGroup
    {
        public string TestGroupName { get; private set; }
        private List<Testcase> testcases;

        private bool testGroupRunning;
        private int totalFailedTestcase;
        private int totalPassedTestcase;
        private int totalTestcase;

        public TestGroup(string testGroupName)
        {
            this.TestGroupName = testGroupName;
            this.testcases = new List<Testcase>();
            this.testGroupRunning = false;
            this.totalFailedTestcase = 0;
            this.totalPassedTestcase = 0;
            this.totalTestcase = 0;
        }

        public void AddTestcase(Testcase testcase)
        {
            totalTestcase++;
            this.testcases.Add(testcase);
        }

        public async Task Run()
        {
            if (this.testGroupRunning)
            {
                throw new Exception("This test group is running, please wait until it not finished");
            }

            this.testGroupRunning = true;

            foreach (Testcase testcase in testcases)
            {
                bool pass;
                
                pass = await testcase.Run();
                
                if (pass)
                {
                    totalPassedTestcase++;
                }
                else
                {
                    totalFailedTestcase++;
                }
            }

            this.testGroupRunning = false;
        }

        public void Reset()
        {
            this.testcases.Clear();
            this.testGroupRunning = false;
            this.totalFailedTestcase = 0;
            this.totalPassedTestcase = 0;
            this.totalTestcase = 0;
            this.totalTestcase = 0;
        }

        public List<string> GetLogs()
        {
            List<string> results = new List<string>();

            //Group test details
            results.Add("Test gorup name : " + TestGroupName);
            results.Add("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            if(totalPassedTestcase == totalTestcase)
                results.Add(string.Format("Testcase Passed : {0}/{1} ", totalPassedTestcase,totalTestcase));
            else
                results.Add(string.Format("Testcase Passed : {0}/{2} !! Testcase Failed : {1}/{2}", totalPassedTestcase, totalFailedTestcase, totalTestcase));
            results.Add("==============================================================================");
            results.Add("");
            results.Add("");
            // get all tesecase individual results
            foreach (Testcase testcase in testcases)
            {
                results.Add("Tastcase Name : " + testcase.TestcaseName);
                results.Add("---------------------------------------------------------------------------");
                results.Add("Logs : " + testcase.getLogs());
                results.Add("");
                  
            }

            return results;
        }
    }
}
