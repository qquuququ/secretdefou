using System;

namespace Archipelago
{
    // Token: 0x02000019 RID: 25
    internal class CustomPDA
    {
        // Token: 0x0600004B RID: 75 RVA: 0x0000415C File Offset: 0x0000235C
        public static void Add(string key, PDAEncyclopedia.Entry entry)
        {
            long id;
            if (ArchipelagoData.Encyclopedia.TryGetValue(key, out id))
            {
                APState.SendLocID(id);
            }
        }
    }
}
