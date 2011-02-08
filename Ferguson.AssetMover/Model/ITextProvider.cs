using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferguson.AssetMover.Client.Model
{
    public delegate void TextInputHandler(string textInput);

    public interface ITextProvider
    {
        event TextInputHandler TextInputOccured;
    }
}
