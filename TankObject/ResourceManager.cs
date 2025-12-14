using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace TankObject
{
    internal class ResourceManager
    {
        private static ResourceManager instance;
        private ResourceManager() { }
        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResourceManager();

                }
                return instance;
            }
        }
        private Dictionary<string, Image> images = new Dictionary<string, Image>();

        public void LoadImage(string name, Image image)
        {
            if (!images.ContainsKey(name))
            {
                images[name] = image;
            }
        }
        public Image GetImage(string name)
        {
            if (images.ContainsKey(name))
            {
                return images[name];
            }

            return null;
        }
        public void ReleaseAll()
    {
        foreach (var image in images.Values)
        {
            image.Dispose(); 
        }
        images.Clear(); 
    }
    }
}
