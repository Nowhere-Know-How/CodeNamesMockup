using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CodeNames
{
    public class Constants
    {
        public static string GET_RANDOM_WORDS_SQL = "SELECT word FROM vocab order by RANDOM() limit 25";
        public static string GET_RANDOM_KEY_CARD  = "SELECT id, firstToMove, data, size FROM keys order by RANDOM() limit 1";
    }
}