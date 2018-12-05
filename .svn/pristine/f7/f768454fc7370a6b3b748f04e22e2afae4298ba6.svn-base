using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FirstFloor.ModernUI.Presentation
{
    /// <summary>
    /// Represents a displayable link.
    /// </summary>
    public class Link
        : Displayable
    {
        private Uri source;

        /// <summary>
        /// Gets or sets the source uri.
        /// </summary>
        /// <value>The source.</value>
        public Uri Source
        {
            get { return this.source; }
            set
            {
                if (this.source != value) {
                    this.source = value;
                    OnPropertyChanged("Source");
                }
            }
        }

        private Geometry _iconData;
        /// <summary>
        /// Gets or sets the icondata
        /// </summary>
        /// <value>The source.</value>
        public Geometry IconData
        {
            get => _iconData;
            set
            {
                if (!Equals(this._iconData, value))
                {
                    this._iconData = value;
                    OnPropertyChanged("IconData");
                }
            }
        }
    }
}
