using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimLookup.Module.Controllers.MariaDB
{
    internal class PreVersionedProperties
    {
        public static Dictionary<int, int> GetProVersions()
        {
            Dictionary<int, int> properties = new Dictionary<int, int>();
            properties.Add(2, 1149);
            properties.Add(18, 1066);
            properties.Add(21, 1080);
            properties.Add(23, 1081);
            properties.Add(37, 1062);
            properties.Add(40, 1065);
            properties.Add(42, 1064);
            properties.Add(43, 1082);
            properties.Add(48, 1085);
            properties.Add(54, 1158);
            properties.Add(64, 1086);
            properties.Add(65, 1087);
            properties.Add(73, 1138);
            properties.Add(75, 1137);
            properties.Add(76, 1111);
            properties.Add(77, 1112);
            properties.Add(79, 1128);
            properties.Add(80, 1136);
            properties.Add(81, 1135);
            properties.Add(82, 1134);
            properties.Add(90, 1074);
            properties.Add(105, 1071);
            properties.Add(109, 1072);
            properties.Add(111, 1084);
            properties.Add(114, 1068);
            properties.Add(116, 1073);
            properties.Add(135, 1061);
            properties.Add(168, 1063);
            properties.Add(182, 1053);
            properties.Add(184, 1047);
            properties.Add(189, 1049);
            properties.Add(190, 1050);
            properties.Add(193, 1055);
            properties.Add(195, 1048);
            properties.Add(196, 1032);
            properties.Add(197, 1056);
            properties.Add(198, 1033);
            properties.Add(199, 1054);
            properties.Add(202, 1057);
            properties.Add(203, 1038);
            properties.Add(204, 1039);
            properties.Add(205, 1058);
            properties.Add(206, 1059);
            properties.Add(207, 1030);
            properties.Add(208, 1051);
            properties.Add(211, 1031);
            properties.Add(212, 1044);
            properties.Add(213, 1034);
            properties.Add(214, 1035);
            properties.Add(215, 1045);
            properties.Add(216, 1036);
            properties.Add(217, 1046);
            properties.Add(218, 1041);
            properties.Add(228, 1096);
            properties.Add(229, 1095);
            properties.Add(233, 1094);
            properties.Add(234, 1090);
            properties.Add(235, 1091);
            properties.Add(236, 1093);
            properties.Add(237, 1092);
            properties.Add(238, 1097);
            properties.Add(239, 1099);
            properties.Add(244, 1130);
            properties.Add(278, 1088);

            return properties;

        }
    }
}
