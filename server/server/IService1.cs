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
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        void UploadFile(RemoteFileInfo request);

        [OperationContract]
        RemoteFileInfo DownloadFile(DownloadRequest request);

        [OperationContract]
        FileInfo[] Masage(string request);

        [OperationContract]
        void Delit(string filname, string request);
    }

    [MessageContract]
    public class DownloadRequest
    {
        [MessageBodyMember]
        public string FileName;

        [MessageBodyMember]
        public string mail;
    }

    [MessageContract]
    public class RemoteFileInfo : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string mail;

        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public long Length;

        [MessageBodyMember(Order = 1)]
        public System.IO.Stream FileByteStream;

        public void Dispose()
        {
            // close stream when the contract instance is disposed. this ensures that stream is closed when file download is complete, since download procedure is handled by the client and the stream must be closed on server.
            // thanks Bhuddhike! http://blogs.thinktecture.com/buddhike/archive/2007/09/06/414936.aspx
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }
}