//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Dungeon_Maker
//{
//    class Treasure
//    {

//        private static string Container(ref Random random)
//        {
//            switch (Dice.d20(ref random))
//            {
//                case 1:
//                case 2: return " is contained in bags.";
//                case 3:
//                case 4: return " is contained in sacks.";
//                case 5:
//                case 6: return " is contained in small coffers.";
//                case 7:
//                case 8: return " is contained in chests.";
//                case 9:
//                case 10: return " is contained in huge chests.";
//                case 11:
//                case 12: return " is contained in pottery jars.";
//                case 13:
//                case 14: return " is contained in metal urns.";
//                case 15:
//                case 16: return " is contained in stone containers.";
//                case 17:
//                case 18: return " is contained in iron trunks.";
//                default: return "";
//            }
//        }

//        private static string Hidden(ref Random random)
//        {
//            switch (Dice.d20(ref random))
//            {
//                case 1:
//                case 2:
//                case 3: return " is hidden by an invisibility spell.";
//                case 4:
//                case 5: return " is hidden by an illusion.";
//                case 6: return " is hidden in a secret space under the container.";
//                case 7:
//                case 8: return " is hidden in a secret compartment in the container.";
//                case 9: return " is hidden inside an oridnary item in plain view.";
//                case 10: return " is disguised to appear as something else.";
//                case 11: return " is hidden under a heap of trash/dung.";
//                case 12:
//                case 13: return " is hidden under a loose stone in the floor.";
//                case 14:
//                case 15: return " is hidden behind a loose stone in the wall.";
//                default: return " is hidden in a secret room nearby.";
//            }
//        }
//    }
//}