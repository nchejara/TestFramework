using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.Framework
{
    public class Testcase
    {
        public delegate Task<bool> TestcaseExecution(Testcase testcase);
        
        public string TestcaseName { get; private set; }
        public TestcaseStatus Status { get; private set; }
        public string Logs;

        public TestcaseExecution execution;
        private List<Testcase> testcase;

        public Testcase(string testcaseName, TestcaseExecution execution)
        {
            this.TestcaseName = testcaseName;
            this.execution = execution;
            this.Logs = string.Empty;
            this.testcase = new List<Testcase>();
            this.Status = TestcaseStatus.NotRun;
        }

        public async Task<bool> Run()
        {
            this.Status = TestcaseStatus.Running;
            bool isPass = false;
            
            try
            {
                isPass = await this.execution(this);
                this.Status = isPass ? TestcaseStatus.Passed : TestcaseStatus.Failed;
                Logs = this.Status.ToString();
            }
            catch (Exception ex)
            {
                isPass = false;
                this.Status = TestcaseStatus.Failed;
                Logs = ex.Message;
            }

            return isPass;
        }

        public void Reset()
        {
            Logs = "";
            this.Status = TestcaseStatus.NotRun;
        }

        public string getLogs()
        {
            return this.Logs;
        }
    }
}
