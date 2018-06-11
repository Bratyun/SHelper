package com.vottakvot.nevskiy.knuckles.Parser.Threads;

import android.database.sqlite.SQLiteDatabase;

import com.vottakvot.nevskiy.knuckles.MainActivity;
import com.vottakvot.nevskiy.knuckles.Parser.Test;
import com.vottakvot.nevskiy.knuckles.Parser.WSTask;
import com.vottakvot.nevskiy.knuckles.Parser.db.DBHelper;
import com.vottakvot.nevskiy.knuckles.Parser.synchronize.Synchro;

/**
 * Created by Shuyning on 10.06.2018.
 */

public class ThreadSynchronize extends Thread {
    private SQLiteDatabase database;

    public ThreadSynchronize(SQLiteDatabase db) {
        database = db;
    }

    public void setDatabase(SQLiteDatabase database) {
        this.database = database;
    }

    @Override
    public void run() {
        Synchro synchro = new Synchro(database);
        DBHelper dbHelper = new DBHelper(MainActivity.getInstance(), false);
        database = dbHelper.getmDataBase();

        for (WSTask task : synchro.getWsTaskList()) {
            dbHelper.insertTask(task, database);
        }

        MainActivity.getInstance().setSyn(true);
    }


}
