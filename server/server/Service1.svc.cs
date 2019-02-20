using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace server
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class Service1 : IService1
    {

        string str;

        public void Delit(string filname, string request)
        {
            string currDirк = Environment.CurrentDirectory.ToString() + @"\Upload\" + request + @"\" + filname;
            File.Delete(currDirк);
            //Console.WriteLine("Файл " + filname + " удалён");
        }

        public RemoteFileInfo DownloadFile(DownloadRequest request)
        {
            str = request.mail;

            string filePath = System.IO.Path.Combine("Upload/" + request.mail, request.FileName);
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);


            //Console.WriteLine("Отправка потока " + request.FileName + " для клиента");
            //Console.WriteLine("Размер " + fileInfo.Length);


            if (!fileInfo.Exists) throw new System.IO.FileNotFoundException("Файл не найден", request.FileName);


            System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);


            RemoteFileInfo result = new RemoteFileInfo();
            result.FileName = request.FileName;
            result.Length = fileInfo.Length;
            result.FileByteStream = stream;
            return result;


        }

        public FileInfo[] Masage(string request)//////////////////////////////
        {
            string currDir = Environment.CurrentDirectory.ToString() + @"/Upload/" + request;
            DirectoryInfo dinfo = new DirectoryInfo(currDir);
           
            return dinfo.GetFiles();
        }

        public void UploadFile(RemoteFileInfo request)
        {

            //Console.WriteLine("Пользователь: " + request.mail);
            //Console.WriteLine("Начать загрузку " + request.FileName);
            //Console.WriteLine("Размер " + request.Length);


            if (!System.IO.Directory.Exists("Upload/" + request.mail)) System.IO.Directory.CreateDirectory("Upload/" + request.mail);


            string filePath = System.IO.Path.Combine("Upload/" + request.mail, request.FileName);
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

            int chunkSize = 2048;
            byte[] buffer = new byte[chunkSize];

            using (System.IO.FileStream writeStream = new System.IO.FileStream(filePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write))
            {
                do
                {

                    int bytesRead = request.FileByteStream.Read(buffer, 0, chunkSize);
                    if (bytesRead == 0) break;


                    writeStream.Write(buffer, 0, bytesRead);
                } while (true);


                //Console.WriteLine("Файл принят!");

                writeStream.Close();
            }
        }
    }
}