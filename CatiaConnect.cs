using System.Runtime.InteropServices;

namespace TechBOM
{
    public class CatiaConnect
    {
        // Статическая переменная, которая будет хранить единственный экземпляр класса
        private static CatiaConnect _instance = new CatiaConnect();
        public INFITF.Application Catia { get; set; }

        // Закрытый конструктор, чтобы предотвратить создание экземпляра вне класса
        private CatiaConnect()
        {
            ConnectCatia();
        }

        // Статический метод для доступа к экземпляру
        public static CatiaConnect Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CatiaConnect();
                }
                return _instance;
            }
        }

        public INFITF.Application ConnectCatia()
        {
            try
            {
                Catia = (INFITF.Application)MarshalCore.GetActiveObject("Catia.Application");
                InitializeCatiaSettings();
                return Catia;
            }
            catch
            {
                return null;
            }
        }

        private void InitializeCatiaSettings()
        {
            if (Catia != null)
            {
                Catia.DisplayFileAlerts = false;
            }
        }

        public bool IsDocumentLoaded()
        {
            try
            {
                _ = Instance.Catia.ActiveDocument;
                //Logger.Info("Trying to get an active document...");
                return true;
            }
            catch (COMException)
            {
                //Logger.Info("There is no active CATIA document");
                return false;
            }
        }

    }
}
