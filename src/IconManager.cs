using System.Runtime.InteropServices;

namespace wallicons
{
    public class IconPosition
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    internal class IconManager
    {
        // https://devblogs.microsoft.com/oldnewthing/20130318-00/?p=4933
        public List<IconPosition> GetIconPositions()
        {
            var result = new List<IconPosition>();

            dynamic app = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));
            var windows = app.Windows;

            const int SWC_DESKTOP = 8;
            const int SWFO_NEEDDISPATCH = 1;
            var hwnd = 0;
            var disp = windows.FindWindowSW(Type.Missing, Type.Missing, SWC_DESKTOP, ref hwnd, SWFO_NEEDDISPATCH);

            var sp = (IServiceProvider)disp;
            var SID_STopLevelBrowser = new Guid("4c96be40-915c-11cf-99d3-00aa004ae837");

            var browser = (IShellBrowser)sp.QueryService(SID_STopLevelBrowser, typeof(IShellBrowser).GUID);
            var view = (IFolderView)browser.QueryActiveShellView();
            var view2 = (IFolderView2)view;

            for (var i = 0; i < view.ItemCount(); i++)
            {
                var item = view2.GetItem(i, typeof(IShellItem).GUID);

                var pidl = view.Item(i);
                view.GetItemPosition(pidl, out var pt);

                var iconPosition = new IconPosition()
                {
                    Name = item.GetDisplayName(SIGDN.SIGDN_DESKTOPABSOLUTEPARSING),
                    X = pt.x,
                    Y = pt.y
                };

                result.Add(iconPosition);
            }

            return result;
        }

        public void SetIconPositions(List<IconPosition> iconPositions)
        {
            dynamic app = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));
            var windows = app.Windows;

            const int SWC_DESKTOP = 8;
            const int SWFO_NEEDDISPATCH = 1;
            var hwnd = 0;
            var disp = windows.FindWindowSW(Type.Missing, Type.Missing, SWC_DESKTOP, ref hwnd, SWFO_NEEDDISPATCH);

            var sp = (IServiceProvider)disp;
            var SID_STopLevelBrowser = new Guid("4c96be40-915c-11cf-99d3-00aa004ae837");

            var browser = (IShellBrowser)sp.QueryService(SID_STopLevelBrowser, typeof(IShellBrowser).GUID);
            var view = (IFolderView)browser.QueryActiveShellView();
            var view2 = (IFolderView2)view;

            for (var i = 0; i < view.ItemCount(); i++)
            {
                var item = view2.GetItem(i, typeof(IShellItem).GUID);

                if (iconPositions.SingleOrDefault(s => s.Name == item.GetDisplayName(SIGDN.SIGDN_DESKTOPABSOLUTEPARSING)) is IconPosition iconPosition)
                {
                    var pidl = view.Item(i);
                    view.GetItemPosition(pidl, out var pt);
                    pt.x = iconPosition.X;
                    pt.y = iconPosition.Y;
                    view.SelectAndPositionItems(1, [pidl], [pt], SVSIF.SVSI_POSITIONITEM);
                }
            }
        }

        [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IServiceProvider
        {
            [return: MarshalAs(UnmanagedType.IUnknown)]
            object QueryService([MarshalAs(UnmanagedType.LPStruct)] Guid service, [MarshalAs(UnmanagedType.LPStruct)] Guid riid);
        }

        // note: for the following interfaces, not all methods are defined as we don't use them here
        [ComImport, Guid("000214E2-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellBrowser
        {
            void _VtblGap1_12(); // skip 12 methods https://stackoverflow.com/a/47567206/403671
            [return: MarshalAs(UnmanagedType.IUnknown)]
            object QueryActiveShellView();
        }

        [ComImport, Guid("cde725b0-ccc9-4519-917e-325d72fab4ce"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFolderView
        {
            void _VtblGap1_3(); // skip 3 methods
            IntPtr Item(int iItemIndex);
            int ItemCount(uint uFlags = 0);
            void _VtblGap2_3(); // skip 2 methods
            void GetItemPosition(IntPtr pidl, out POINT ppt);
            void _VtblGap1_4(); // skip 4 methods
            void SelectAndPositionItems(int cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] apidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] POINT[] apt, SVSIF dwFlags);
        }

        [ComImport, Guid("1af3a467-214f-4298-908e-06b03e0b39f9"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFolderView2
        {
            void _VtblGap1_26(); // skip 14 (IFolderView) + 12 methods
            IShellItem GetItem(int iItemIndex, [MarshalAs(UnmanagedType.LPStruct)] Guid riid);
            // more undefined methods
        }

        [ComImport, Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItem
        {
            [return: MarshalAs(UnmanagedType.IUnknown)]
            object BindToHandler(System.Runtime.InteropServices.ComTypes.IBindCtx pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid bhid, [MarshalAs(UnmanagedType.LPStruct)] Guid riid);
            IShellItem GetParent();
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string GetDisplayName(SIGDN sigdnName);
            // more undefined methods
        }

        private struct POINT
        {
            public int x;
            public int y;
        }

        private enum SIGDN
        {
            SIGDN_NORMALDISPLAY,
            SIGDN_PARENTRELATIVEPARSING,
            SIGDN_DESKTOPABSOLUTEPARSING,
            SIGDN_PARENTRELATIVEEDITING,
            SIGDN_DESKTOPABSOLUTEEDITING,
            SIGDN_FILESYSPATH,
            SIGDN_URL,
            SIGDN_PARENTRELATIVEFORADDRESSBAR,
            SIGDN_PARENTRELATIVE,
            SIGDN_PARENTRELATIVEFORUI
        }

        [Flags]
        private enum SVSIF
        {
            SVSI_DESELECT = 0,
            SVSI_SELECT = 0x1,
            SVSI_EDIT = 0x3,
            SVSI_DESELECTOTHERS = 0x4,
            SVSI_ENSUREVISIBLE = 0x8,
            SVSI_FOCUSED = 0x10,
            SVSI_TRANSLATEPT = 0x20,
            SVSI_SELECTIONMARK = 0x40,
            SVSI_POSITIONITEM = 0x80,
            SVSI_CHECK = 0x100,
            SVSI_CHECK2 = 0x200,
            SVSI_KEYBOARDSELECT = 0x401,
            SVSI_NOTAKEFOCUS = 0x40000000
        }
    }
}