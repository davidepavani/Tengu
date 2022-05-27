using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Shared.Cheet.CheetCore;

namespace Tengu.Shared.Cheet
{
    public interface ICheet : ICheet<Key>
    {
        /// <summary>
        ///     KeyEventHandler to be used to subscribe to user keyboard events from WPF elements.
        /// </summary>
        void OnKeyDown(object sender, KeyEventArgs e);
    }
}
