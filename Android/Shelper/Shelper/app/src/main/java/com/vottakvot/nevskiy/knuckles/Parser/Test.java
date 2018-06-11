package com.vottakvot.nevskiy.knuckles.Parser;

import android.content.ContentValues;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.util.Log;

import com.vottakvot.nevskiy.knuckles.Controller;
import com.vottakvot.nevskiy.knuckles.Parser.data.Data;
import com.vottakvot.nevskiy.knuckles.Parser.data.DataTag;
import com.vottakvot.nevskiy.knuckles.Parser.data.Element;
import com.vottakvot.nevskiy.knuckles.Parser.data.Result;
import com.vottakvot.nevskiy.knuckles.Parser.db.DBHelper;
import com.vottakvot.nevskiy.knuckles.Parser.synchronize.Synchro;

import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;



/**
 * Created by Shuyning on 03.06.2018.
 */

public class Test extends Thread {
    final String LOG_TAG = "myLogs";

    private static DBHelper dbHelper;

    private String name = "Lifehacker-new";
    private String link = "https://proglib.io/";

    private String baseTag = "div";
    private String where = "in";
    private String dist = "first";

    private static List<WSTask> wsTaskList;

    private static List<String> listName = new ArrayList<>();
    private static List<String> listResult = new ArrayList<>();

    public Test(DBHelper dbH) {
        dbHelper = dbH;
    }

    @Override
    public void run() {
        //while(true){
        Log.d(LOG_TAG, "<--------------------NEW INFO------------------->");
        System.out.println("\n\n\n<--------------------NEW INFO------------------->");
        process();
        //}
    }

    private void process(){
        //WSTask task1 = new WSTask(name, link);
        WSTask task = null;

        if (task == null) {
            task = new WSTask(name, link);

            setDataToTwoStep(task, true);
            addDataList(1, 1, "container", task);
            task.process();
        } else {
            task.process();
        }

        for (Result result : task.getResultResp()) {
            System.out.println(result.getResult());
        }

        workWithBD(task);
    }

    public static List<String> getListName() {
        return listName;
    }

    public static List<String> getListResult() {
        return listResult;
    }

    private void setDataToTwoStep(WSTask task, boolean isData) {
        task.startStepTwo(baseTag, where, dist);

        if (isData) {
            task.setDataToTwoStep("auto", "class", "inside", "td_block_inner");
        }
    }

    private void addDataList(int quantity, int numbeReturneData, String start, WSTask task) {
        List<DataTag> dataTags = new ArrayList<>();

        DataTag dataTag = new DataTag();
        dataTag.addDataList(quantity, numbeReturneData, start, getTestElements());
        dataTag.addDataList(quantity, numbeReturneData, start, getTestElements2());
        dataTag.addDataList(quantity, numbeReturneData, start, getTestElements3());

        dataTags.add(dataTag);

        task.addDataTagToElementContainer(dataTags);
    }

    private List<Element> getTestElements() {
        List<Element> list = new ArrayList<>();
        Element element = new Element();

        element.setTag("h3");
        element.seteDistance("first");
        element.seteLocation("in");

        Data data = new Data();
        data.setPurpose("auto");
        data.setProperties("class");
        data.setHowSearch("inside");
        data.setValue("entry-title td-module-title");

        element.addDataList(data);

        Data data2 = new Data();
        data2.setPurpose("auto");
        data2.setProperties("");
        data2.setHowSearch("inside");
        data2.setValue("");

        element.addDataList(data2);

        list.add(element);

        return list;
    }

    private List<Element> getTestElements2() {
        List<Element> list = new ArrayList<>();
        Element element = new Element();

        element.setTag("a");
        element.seteDistance("first");
        element.seteLocation("in");

        Data data = new Data();
        data.setPurpose("auto");
        data.setProperties("href");
        data.setHowSearch("inside");
        data.setValue("");

        element.addDataList(data);

        list.add(element);

        return list;
    }

    private List<Element> getTestElements3() {
        List<Element> list = new ArrayList<>();
        Element element = new Element();

        element.setTag("div");
        element.seteDistance("first");
        element.seteLocation("in");

        Data data = new Data();
        data.setPurpose("auto");
        data.setProperties("class");
        data.setHowSearch("inside");
        data.setValue("td-excerpt");

        element.addDataList(data);

        Data data2 = new Data();
        data2.setPurpose("auto");
        data2.setProperties("");
        data2.setHowSearch("inside");
        data2.setValue("");

        element.addDataList(data2);

        list.add(element);

        return list;
    }

    public static List<WSTask> getWsTaskList() {
        return wsTaskList;
    }

    private void workWithBD(WSTask task) {
        // подключаемся к БД
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        //dbHelper.onCreate(db);

        ContentValues contentValues = new ContentValues();

        insert(task, db);

        wsTaskList = dbHelper.getTasksList(db);

        for (WSTask task1: wsTaskList) {
            listName.add(task.getName());
            for (Result result: task.getResultResp()) {
                listResult.add(result.getResult());
            }
        }

        System.out.println("Ok");

    }

    public static void insert(WSTask task, SQLiteDatabase db) {
        //Log.d(LOG_TAG, "--- Insert task " + task.getName() + ": ---");
        dbHelper.insertTask(task, db);
    }

    public static boolean testConnection(String url){
        try {
            Document document = Jsoup.connect(url).get();
            return true;
        }catch (IOException e){
            return false;
        }
    }

}
