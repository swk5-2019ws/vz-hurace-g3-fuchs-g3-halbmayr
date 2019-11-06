using System;
using System.Collections.Generic;

namespace InsertScriptGenerator.Core
{
    internal class Image
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[Image] ([Id], [Content]) "
                + $"VALUES ({Id}, CONVERT(VARBINARY(MAX), '{Convert.ToBase64String(Content)}'));";
        }
    }
}
