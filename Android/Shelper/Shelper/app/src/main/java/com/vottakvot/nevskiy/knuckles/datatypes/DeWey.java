package com.vottakvot.nevskiy.knuckles.datatypes;

/**
 * Created by Aurelius on 09.06.2018.
 */

public class DeWey {
    public String auto;
    public String purpose;
    public String properties;
    public String howSearch;

    public DeWey(){
        auto="";
        purpose="";
        properties="";
        howSearch="";
    }

    public DeWey(String dewey1, String dewey2,String dewey3,String dewey4){
        this.auto=dewey1;
        this.purpose=dewey2;
        this.properties=dewey3;
        this.howSearch=dewey4;
    }
}
