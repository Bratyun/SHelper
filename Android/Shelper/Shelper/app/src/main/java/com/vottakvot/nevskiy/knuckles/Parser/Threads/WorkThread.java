package com.vottakvot.nevskiy.knuckles.Parser.Threads;

import android.database.sqlite.SQLiteDatabase;

import com.vottakvot.nevskiy.knuckles.Parser.WSTask;
import com.vottakvot.nevskiy.knuckles.Parser.data.Result;
import com.vottakvot.nevskiy.knuckles.Parser.db.DBHelper;

import junit.framework.Assert;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Shuyning on 11.06.2018.
 */

public class WorkThread extends Thread {
    private SQLiteDatabase database;
    private List<WSTask> wsTaskList;
    private DBHelper dbHelper;

    static List<String> nameList = new ArrayList<>();
    static List<String> resultList = new ArrayList<>();

    public WorkThread(DBHelper dbHelper) {
        this.dbHelper =dbHelper;
        database = dbHelper.getWritableDatabase();
        System.out.println();
    }

    public void setDatabase(SQLiteDatabase database) {
        this.database = database;
    }

    @Override
    public void run() {
        wsTaskList = dbHelper.getTasksList(database);
        String r = "";

        for (WSTask task: wsTaskList) {
            nameList.add(task.getName());
            for (Result result: task.getResultList()) {
                if (result.getResult().equals("")){
                    r += "Ничего не найдено\n\n";
                }else {
                    r += result.getResult() + "\n\n";
                }
            }
            /*WSTask wsTask = new WSTask(task.getName(), task.getLink());

            wsTask.setContainerStepTwo(task.getContainerStepTwo());
            wsTask.setElementContainer(task.getElementContainer());

            wsTask.process();*/

            resultList.add(r);
            r = "";
        }
    }

    public static List<String> getNameTask(){
        return nameList;
    }

    public static List<String> getResultList(){
        return resultList;
    }
}
