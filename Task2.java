public class Task2 {
    //Return True is array2 is a subsequence of array1
    static boolean isSubsequence(int array1[], int array2[], int m, int n) {
        int i = 0;
        int j = 0;
        for(i = 0; i < n;  i++) {
            //if the loop doen not break, then array2[i] is not present in array1[j]
            for(j = 0; j < m; j++) {
                if(array2[i] == array1[j]) {
                    break;
                }
            }
            
            if(j == m) {
                return false;
            }
        }

        //all ellement in array2[] are present in array1[]
        return true;
    }

    public static void main(String[] args) {
        int array1[] = {1, 11, 23, 21, 4, 7};
        int array2[] = {1, 23, 7};

        int m = array1.length;
        int n = array2.length;

        if(isSubsequence(array1, array2, m, n)) {
           System.out.println("array2[] is subsequence of array1[]");

        }else {
            System.out.println("array2[] is not a subsequence of array1[]");
        }
    }
}
