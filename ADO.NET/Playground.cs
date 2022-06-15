﻿using MinionNames;
using AddMinion;
using System;
using System.Threading.Tasks;
using VillainNames;
using InitialSetup;

namespace ADO.NET
{
    public class Playground
    {
        private async static Task Main()
        {
            Console.WriteLine(Constants.TO_CLOSE_APP);
            Console.Write(Constants.ENTER_TASK_NUMBER);
            var taskNumber = Console.ReadLine();

            while (taskNumber != Constants.CLOSE_APP_FLAG)
            {
                switch (taskNumber)
                {
                    case "01":
                        await Task01.RunTask01();
                        break;
                    case "02":
                        await Task02.RunTask02();
                        break;
                    case "03":
                        await Task03.RunTask03();
                        break;
                    case "04":
                        await Task04.RunTask04();
                        break;
                    default:
                        Console.WriteLine(Constants.ENTER_EXISTING_TASK_NUMBER);
                        break;
                }
                Console.Write(Constants.ENTER_TASK_NUMBER);
                taskNumber = Console.ReadLine();
            }
        }
    }
}
