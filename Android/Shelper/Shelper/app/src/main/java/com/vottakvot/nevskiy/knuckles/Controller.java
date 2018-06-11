package com.vottakvot.nevskiy.knuckles;

import android.database.sqlite.SQLiteDatabase;

import com.vottakvot.nevskiy.knuckles.Parser.Test;
import com.vottakvot.nevskiy.knuckles.Parser.WSTask;
import com.vottakvot.nevskiy.knuckles.Parser.db.DBHelper;
import com.vottakvot.nevskiy.knuckles.datatypes.DeWey;
import com.vottakvot.nevskiy.knuckles.datatypes.Uganda;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Aurelius on 24.05.2018.
 */

public class Controller {
    private  MainActivity mainActivity = MainActivity.getInstance();
    private final DBHelper dbHelper = new DBHelper(mainActivity, false);

    private SQLiteDatabase database = dbHelper.getWritableDatabase();

    private List<WSTask> wsTaskList;

    public void setWsTaskList(List<WSTask> wsTaskList) {
        this.wsTaskList = wsTaskList;
    }

    public static List<String> grouplist = new ArrayList<String>(){
        {
            add("mail.ru");
            add("1xbet.com");
            add("joycasino.com");
        }
    };
    public static List<String> grouptext = new ArrayList<String>(){
        {
            add("Братишка, я тебе спутник принес");
            add("Ебемся в жопу без перерывов и выходных");
            add("Отборные пидорасы со всего рунета, теперь и в вашем кино");
        }
    };
    public static List<DeWey> deWeys = new ArrayList<DeWey>(){
        {
        }
    };

    public static List<List<DeWey>> glists = new ArrayList<List<DeWey>>(){
        {

        }
    };

    public static List<Uganda> ugandas = new ArrayList<Uganda>(){
        {

        }
    };

}
