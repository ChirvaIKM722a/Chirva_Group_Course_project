﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Collections;

namespace Chirva_Group_Course_project
{
    class MajorWork
    {
        // Вміст робочого об'єкта
        // Поля
        private System.DateTime TimeBegin;
        private string Data; //вхідні дані
        private string Result; // Поле результату
        public bool Modify;
        private int Key;

        public Stack myStack = new Stack();
        public string[] myArr = new string[100];

        private string SaveFileName;// ім’я файлу для запису
        private string OpenFileName;// ім’я файлу для читання

        public Queue myQueue = new Queue();
        public string[] smyQueue = new string[100];

        public void WriteSaveFileName(string S)// метод запису даних в об'єкт
        {
            this.SaveFileName = S;// запам'ятати ім’я файлу для запису
        }
        public void WriteOpenFileName(string S)
        {
            this.OpenFileName = S;// запам'ятати ім’я файлу для відкриття
        }

        // Методи
        public void SetTime() // метод запису часу початку роботи програми
        {
            this.TimeBegin = System.DateTime.Now;
        }
        public System.DateTime GetTime() // Метод отримання часу завершення програми
        {
            return this.TimeBegin;
        }
            public void Write(string D)// метод запису даних в об'єкт.
        {
            this.Data = D;
        }
        public string Read()
        {
            return this.Result;// метод відображення результату
        }
        public void Task() // метод реалізації програмного завдання
        {
            if (this.Data.Length > 5)
            {
                this.Result = Convert.ToString(true);

            }
            else
            {
                this.Result = Convert.ToString(false);
            }
            this.Modify = true;
        }
        public void SaveToFile() // Запис даних до файлу
        {
            if (!this.Modify)
                return;
            try
            {
                Stream S; // створення потоку
                if (File.Exists(this.SaveFileName))// існує файл?
                    S = File.Open(this.SaveFileName, FileMode.Append);// Відкриття файлу для збереження
                else
                    S = File.Open(this.SaveFileName, FileMode.Create);// створити файл
                Buffer D = new Buffer(); // створення буферної змінної
                D.Data = this.Data;
                D.Result = Convert.ToString(this.Result);
                D.Key = Key;
                Key++;
                BinaryFormatter BF = new BinaryFormatter();
                BF.Serialize(S, D);
                S.Flush(); // очищення буфера потоку
                S.Close(); // закриття потоку
                this.Modify = false; // Заборона повторного запису
            }
            catch
            {

                MessageBox.Show("Помилка роботи з файлом");
            }
        }

        public void ReadFromFile(System.Windows.Forms.DataGridView DG) 
        {
            try
            {
                if (!File.Exists(this.OpenFileName))
                {
                    MessageBox.Show("Файлу немає");
                    return;
                }
                Stream S;
                S = File.Open(this.OpenFileName, FileMode.Open);
                Buffer D;
                object O;
                BinaryFormatter BF = new BinaryFormatter();
                System.Data.DataTable MT = new System.Data.DataTable();
                System.Data.DataColumn cKey = new

                System.Data.DataColumn("Ключ");// формуємо колонку "Ключ"
                System.Data.DataColumn cInput = new

                System.Data.DataColumn("Вхідні дані");// формуємо колонку "Вхідні
                System.Data.DataColumn cResult = new System.Data.DataColumn("Результат");

                MT.Columns.Add(cKey);// додавання ключа
                MT.Columns.Add(cInput);// додавання вхідних даних
                MT.Columns.Add(cResult);// додавання результату


                    while (S.Position < S.Length)
                    {
                        O = BF.Deserialize(S); // десеріалізація
                        D = O as Buffer;
                        if (D == null) break;
                        System.Data.DataRow MR;
                        MR = MT.NewRow();
                        MR["Ключ"] = D.Key; // Занесення в таблицю номер
                        MR["Вхідні дані"] = D.Data;
                        MR["Результат"] = D.Result;
                        MT.Rows.Add(MR);
                        
                    }
                    DG.DataSource = MT;
                    S.Close(); // закриття
            }
            catch
            {
                MessageBox.Show("Помилка файлу");
            }
        }
        public void Generator() // метод формування ключового поля
        {
            try
            {
                if (!File.Exists(this.SaveFileName)) 
                {
                    Key = 1;
                    return;
                }
                Stream S; // створення потоку
                S = File.Open(this.SaveFileName, FileMode.Open);
                Buffer D;
                object O; 
                BinaryFormatter BF = new BinaryFormatter();
                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S);
                    D = O as Buffer;
                    if (D == null) break;
                    Key = D.Key;
                }
                Key++;
                S.Close();
            }
            catch
            {
                MessageBox.Show("Помилка файлу"); 
            }
        }
        public bool SaveFileNameExists()
        {
            if (this.SaveFileName == null)
                return false;
            else return true;
        }

        public void NewRec() 
        {
            this.Data = "";
            this.Result = null;
            this.TimeBegin = new DateTime(); 
            this.Modify = false; 
            this.Key = 0; 
            this.SaveFileName = null; 
            this.OpenFileName = null; 
        }

        public void Find(string Num) 
        {
            int N;
            try
            {
                N = Convert.ToInt16(Num);
            }
            catch
            {
                MessageBox.Show("помилка пошукового запиту");
            
                      return;
            }

            try
            {
                if (!File.Exists(this.OpenFileName))
                {
                    MessageBox.Show("файлу немає");
                
                       return;
                }
                Stream S; 
                S = File.Open(this.OpenFileName, FileMode.Open); 
                Buffer D;
                object O; 
                BinaryFormatter BF = new BinaryFormatter(); 
            
                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S);
                    D = O as Buffer;
                    if (D == null) break;
                    if (D.Key == N)

                    {
                        string ST;
                        ST = "Запис містить:" + (char)13 + "No" + Num + "Вхідні дані:" +

                        D.Data + "Результат:" + D.Result;

                        MessageBox.Show(ST, "Запис знайдена");

                      S.Close();
                        return;
                    }
                }
                S.Close();
                MessageBox.Show("Запис не знайдена");
            }
            catch
            {
                MessageBox.Show("Помилка файлу");
            }
        }

        private string SaveTextFileName;// ім'я файлу для запису текстового файлу
        public void WriteSaveTextFileName(string S)
        {
            this.SaveTextFileName = S;
        }
        public bool SaveTextFileNameExists()
        {
            if (this.SaveTextFileName == null)
                return false;
            else return true;
        }

        public string ReadSaveTextFileName()
        {
            return SaveTextFileName;
        }

        public void SaveToTextFile(string name, System.Windows.Forms.DataGridView D)
        {
            try
            {
                System.IO.StreamWriter textFile;
                if (!File.Exists(name))
                {
                    textFile = new System.IO.StreamWriter(name);
                }
                else
                {
                    textFile = new System.IO.StreamWriter(name, true);
                }
                for (int i = 0; i < D.RowCount - 1; i++)
                {
                    textFile.WriteLine("{0};{1};{2}", D[0, i].Value.ToString(), D[1, i].Value.ToString(), D[2, i].Value.ToString());

                }
                textFile.Close();
            }
            catch
            {
                MessageBox.Show("Помилка роботи з файлом ");
            }
        }
        private string OpenTextFileName;
        public void WriteOpenTextFileName(string S)
        {
            this.OpenTextFileName = S;
        }



    }
  
}

