using System.Runtime.InteropServices;

namespace TechBOM
{
    public class CatiaConnect
    {
        // Static variable that will store the only instance of the class
        private static CatiaConnect _instance = new();

        public INFITF.Application Catia { get; set; }

        // Closed constructor to prevent creating an instance outside the class
        private CatiaConnect()
        {
            Catia = ConnectCatia();
            InitializeCatiaSettings();
        }

        // Static method to access the instance
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
            INFITF.Application catia;
            try
            {
                catia = (INFITF.Application)MarshalCore.GetActiveObject("Catia.Application");
                return catia;
            }
            catch
            {
                return null;
            }
        }

        private void InitializeCatiaSettings()
        {
            Catia.DisplayFileAlerts = false;
        }

        public bool IsDocumentLoaded()
        {
            try
            {
                _ = Instance.Catia.ActiveDocument;
                return true;
            }
            catch (COMException)
            {
                return false;
            }
        }

    }
}
