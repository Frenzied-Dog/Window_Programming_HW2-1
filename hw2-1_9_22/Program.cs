using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace hw2_1__9_22 {
    internal class Program {
        class Course {
            public Course(string i, int s, int c) {
                id = i; score = s; credit = c;
                gpa_o = score >= 80 ? 4.0 : score >= 70 ? 3.0 : score >= 60 ? 2.0 : score >= 50 ? 1.0 : 0;
                gpa_n = score >= 90 ? 4.3 : score >= 85 ? 4.0 : score >= 80 ? 3.7 : score >= 77 ? 3.3 : score >= 73 ? 3.0 :
                                          score >= 70 ? 2.7 : score >= 67 ? 2.3 : score >= 63 ? 2.0 : score >= 60 ? 1.7 : 0;
                rank = score >= 90 ? "A+" : score >= 85 ? "A" : score >= 80 ? "A-" : score >= 77 ? "B+" : score >= 73 ? "B" :
                                          score >= 70 ? "B-" : score >= 67 ? "C+" : score >= 63 ? "C" : score >= 60 ? "C-" : "F";
            }
            public string id;
            public int score, credit;
            public double gpa_o, gpa_n;
            public string rank;
            public void update() {
                gpa_o = score >= 80 ? 4.0 : score >= 70 ? 3.0 : score >= 60 ? 2.0 : score >= 50 ? 1.0 : 0;
                gpa_n = score >= 90 ? 4.3 : score >= 85 ? 4.0 : score >= 80 ? 3.7 : score >= 77 ? 3.3 : score >= 73 ? 3.0 :
                                          score >= 70 ? 2.7 : score >= 67 ? 2.3 : score >= 63 ? 2.0 : score >= 60 ? 1.7 : 0;
            }
        }

        static void Main(string[] args) {
            bool exit = false;
            List<Course> courses = new();
            while (!exit) {
                Console.WriteLine("## 成績計算器 ##\n1. 新增科目(create)\n2. 刪除科目(delete)\n3. 更新科目(update)\n4. 列印成績單(print)\n5. 退出選單(exit)");
                Console.Write("輸入要執行的指令操作: ");
                string[] cmds = Console.ReadLine().Split(' ');
                string cmd = cmds[0];
                string id;
                Course prefind;

                switch (cmd) {
                case "create":
                case "update":
                    if (cmds.Length != 4) goto default;
                    id = cmds[1];
                    int score = Convert.ToInt32(cmds[2]);
                    if (score > 100 || score < 0) goto scoreError;
                    int credit = Convert.ToInt32(cmds[3]);
                    if (credit > 10 || credit < 0) goto creditError;

                    prefind = courses.FirstOrDefault(x => x.id == id, new Course("", 0, 0));

                    if (cmd == "create") {
                        if (prefind.id != "") {
                            Console.WriteLine("科目已存在"); break;
                        }
                        courses.Add(new Course(id, score, credit));
                        Console.WriteLine("科目已新增");
                    } else {
                        if (prefind.id == "") goto CourseNotExist;
                        prefind.score = score;
                        prefind.credit = credit;
                        prefind.update();
                        Console.WriteLine("科目已更新");
                    }
                    courses.Sort((x, y) => y.score - x.score);
                    break;
                case "delete":
                    if (cmds.Length != 2) goto default;
                    id = cmds[1];
                    prefind = courses.FirstOrDefault(x => x.id == id, new Course("", 0, 0));

                    if (prefind.id == "") goto CourseNotExist;
                    courses.Remove(prefind);
                    Console.WriteLine("科目已刪除");
                    break;
                case "print":
                    if (cmds.Length != 1) goto default;
                    int t_credit = 0, credit_get = 0;
                    double sum_score = 0, sum_gpa_n = 0, sum_gpa_o = 0;
                    Console.WriteLine("我的成績單:\n編號\t科目代碼 分數\t等第\t學分數");
                    int i = 1;
                    foreach (Course c in courses) {
                        t_credit += c.credit;
                        if (c.score >= 60) credit_get += c.credit;
                        sum_score += c.score * c.credit;
                        sum_gpa_n += c.gpa_n * c.credit;
                        sum_gpa_o += c.gpa_o * c.credit;

                        Console.WriteLine($"{i}\t{c.id}\t {c.score}\t{c.rank}\t{c.credit}");
                        i++;
                    }
                    Console.WriteLine($"總平均: {(int)(sum_score * 100 / t_credit + 0.5) / 100.0}\nGPA: {(int)(sum_gpa_o * 10 / t_credit + 0.5) / 10.0}/4.0 (舊制), {(int)(sum_gpa_n * 10 / t_credit + 0.5) / 10.0}/4.3 (新制)\n實拿學分數/總學分數: {credit_get}/{t_credit}");

                    break;
                case "exit":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("指令格式不符! 請重新輸入!");
                    break;
                scoreError:
                    Console.WriteLine("成績分數異常! 請重新輸入!");
                    break;
                creditError:
                    Console.WriteLine("學分數異常! 請重新輸入!");
                    break;
                CourseNotExist:
                    Console.WriteLine("科目不存在");
                    break;
                }

                Console.WriteLine();
            }
        }
    }
}