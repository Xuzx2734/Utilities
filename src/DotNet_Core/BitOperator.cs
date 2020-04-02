
public class BitExtension{

    ///判断是否为偶数
    public static bool IsEven(this int val){
        return (val & 1) == 0;
    }

    ///是否为2的幂次方
    public static bool IsLog2(this int val){
        return (val & (val -1)) == 0;
    }

    

}