package com.vottakvot.nevskiy.knuckles.Parser.db;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.SQLException;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.os.Environment;
import android.util.Log;

import com.vottakvot.nevskiy.knuckles.Parser.WSTask;
import com.vottakvot.nevskiy.knuckles.Parser.data.ContainerPath;
import com.vottakvot.nevskiy.knuckles.Parser.data.Data;
import com.vottakvot.nevskiy.knuckles.Parser.data.DataTag;
import com.vottakvot.nevskiy.knuckles.Parser.data.Element;
import com.vottakvot.nevskiy.knuckles.Parser.data.ElementPath;
import com.vottakvot.nevskiy.knuckles.Parser.data.Result;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.ArrayList;
import java.util.List;


/**
 * Created by Shuyning on 03.06.2018.
 */

public class DBHelper extends SQLiteOpenHelper {
    final String LOG_TAG = "myLogs";

    private static String DB_NAME = "wstask.db";
    private static String DB_PATH = "";
    private static final int DB_VERSION = 1;

    private SQLiteDatabase db;

    private SQLiteDatabase mDataBase;
    private final Context mContext;
    private boolean mNeedUpdate = false;
    boolean syn = false;

    public DBHelper(Context context, boolean synchronize) {
        super(context, DB_NAME, null, DB_VERSION);
        syn = synchronize;

        if (!synchronize) {
        if (android.os.Build.VERSION.SDK_INT >= 17)
            DB_PATH = context.getApplicationInfo().dataDir + "/databases/";
        else
            DB_PATH = "/data/data/" + context.getPackageName() + "/databases/";
        }
        this.mContext = context;

        if (synchronize) {
            DB_PATH = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS).getAbsolutePath() + "/tasks.db";
        }

        openDataBase();

        if (!synchronize) {
            this.getReadableDatabase();
        }
    }

    public void updateDataBase() throws IOException {
        if (mNeedUpdate) {
            File dbFile = new File(DB_PATH + DB_NAME);
            if (dbFile.exists())
                dbFile.delete();

            copyDataBase();

            mNeedUpdate = false;
        }
    }

    public SQLiteDatabase getmDataBase() {
        return mDataBase;
    }

    private boolean checkDataBase() {
        File dbFile = new File(DB_PATH + DB_NAME);
        return dbFile.exists();
    }

    private void copyDataBase() {
        if (!checkDataBase()) {
            this.getReadableDatabase();
            this.close();
            try {
                copyDBFile();
            } catch (IOException mIOException) {
                throw new Error("ErrorCopyingDataBase");
            }
        }
    }

    private void copyDBFile() throws IOException {
        InputStream mInput = mContext.getAssets().open(DB_NAME);
        //InputStream mInput = mContext.getResources().openRawResource(R.raw.info);
        OutputStream mOutput = new FileOutputStream(DB_PATH + DB_NAME);
        byte[] mBuffer = new byte[1024];
        int mLength;
        while ((mLength = mInput.read(mBuffer)) > 0)
            mOutput.write(mBuffer, 0, mLength);
        mOutput.flush();
        mOutput.close();
        mInput.close();
    }

    public boolean openDataBase() throws SQLException {
        if (syn) {
            mDataBase = SQLiteDatabase.openDatabase(DB_PATH /*+ DB_NAME*/, null, SQLiteDatabase.CREATE_IF_NECESSARY);
        }else {
            mDataBase = SQLiteDatabase.openDatabase(DB_PATH + DB_NAME, null, SQLiteDatabase.CREATE_IF_NECESSARY);
        }
        return mDataBase != null;
    }

    @Override
    public synchronized void close() {
        if (mDataBase != null)
            mDataBase.close();
        super.close();
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        db.execSQL("CREATE TABLE `data` ( `id` INTEGER NOT NULL, `container_id` INTEGER NOT NULL, `purpose` TEXT NOT NULL, `properties` TEXT, `howSearch` TEXT NOT NULL, `value` TEXT, `Element_id` INTEGER, PRIMARY KEY(`id`) )");
        db.execSQL("CREATE TABLE `container_path` ( `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `tag` TEXT NOT NULL, `eLocation` TEXT NOT NULL, `eDistance` TEXT NOT NULL, `task_id` INTEGER NOT NULL )");
        db.execSQL("CREATE TABLE `data_tag` ( `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `quantity` INTEGER NOT NULL, `numbeReturneData` INTEGER NOT NULL, `start` TEXT NOT NULL, `ElementPath_id` INTEGER NOT NULL )");
        db.execSQL("CREATE TABLE `element` ( `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `tag` TEXT NOT NULL, `eLocation` TEXT NOT NULL, `eDistance` TEXT NOT NULL, `Data_Tag_id` INTEGER NOT NULL )");
        db.execSQL("CREATE TABLE `element_path` ( `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `name` TEXT NOT NULL, `task_id` INTEGER )");
        db.execSQL("CREATE TABLE `result` ( `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `task_id` INTEGER NOT NULL, `result` TEXT )");
        db.execSQL("CREATE TABLE `tasks` ( `id` integer NOT NULL PRIMARY KEY AUTOINCREMENT, `name` TEXT NOT NULL, `link` TEXT NOT NULL, `ContainerPath_id` INTEGER, `ElementPath_id` INTEGER )");
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        if (newVersion > oldVersion)
            mNeedUpdate = true;
    }

    ////////////////////////////////////////////////////
    // Add info
    ///////////////////////////////////////////////////

    public void insertTask(WSTask task, SQLiteDatabase database){
        db = database;

        String task_id;
        String containerPath_id;
        String elementPath_id;

        ContentValues contentValues = getContentBaseInfo(task);
        long tasks_ID = db.insert("tasks", null, contentValues);
        Log.d(LOG_TAG, "Base information inserted, ID = " + contentValues.getAsString("name"));
        task_id = getTaskIdByNameAndLink(task.getName(), task.getLink());

        ContentValues contentValues1 = getContentContainerPath(task, task_id);
        long container_path_ID = db.insert("container_path", null, contentValues1);
        Log.d(LOG_TAG, "ContainerPath inserted, ID = " + container_path_ID);
        containerPath_id = getContainerIdByTaskId(task_id);

        ContentValues contentValues2 = getContentElementPath(task, task_id);
        long element_path_ID = db.insert("element_path", null, contentValues2);
        Log.d(LOG_TAG, "ElementPath inserted, ID = " + element_path_ID);
        elementPath_id = getElementPathIdByTaskId(task_id);

        ContentValues contentValues3 = getUpdateContentBaseInfo(task, Integer.parseInt(containerPath_id), Integer.parseInt(elementPath_id));
        long base_update_ID = db.update("tasks", contentValues3, null, null);
        Log.d(LOG_TAG, "Base information updating, ID = " + base_update_ID);

        for (DataTag dataTag: task.getElementContainer().getDataTagList()) {
            ContentValues contentValues4 = getContentDataTag(elementPath_id, dataTag);
            long dataTagID = db.insert("data_tag", null, contentValues4);
            Log.d(LOG_TAG, "DataTag contained inserted, ID = " + dataTagID);
            String dataTagId = getDataTagId();

            insertElementsDataTag(dataTagId, dataTag);
        }

        for (Data data: task.getContainerStepTwo().getDataList()) {
            ContentValues contentValues5 = getContentDataContainerPath(containerPath_id, data);
            long dataPathID = db.insert("data", null, contentValues5);
            Log.d(LOG_TAG, "Data contained inserted, ID = " + dataPathID);
        }

        for (Result result: task.getResultList()) {
            ContentValues contentValues6 = getContentResult(result, task_id);
            long result_ID = db.insert("result", null, contentValues6);
            Log.d(LOG_TAG, "Result inserted, ID = " + result_ID);
        }

    }

    private ContentValues getContentContainerPath(WSTask task, String taskId){
        ContentValues contentValues = new ContentValues();

        contentValues.put("tag", task.getContainerStepTwo().getTag());
        contentValues.put("eLocation", task.getContainerStepTwo().getWhere());
        contentValues.put("eDistance", task.getContainerStepTwo().getDistance());
        contentValues.put("task_id", taskId);

        return contentValues;
    }

    private ContentValues getContentDataTag(String elementPathId, DataTag dataTag){
        ContentValues contentValues = new ContentValues();

        contentValues.put("quantity", dataTag.getQuantity());
        contentValues.put("numbeReturneData", dataTag.getNumbeReturneData());
        contentValues.put("start", dataTag.getStart());
        contentValues.put("ElementPath_id", elementPathId);

        return contentValues;
    }

    private ContentValues getContentElementPath(WSTask task, String taskId){
        ContentValues contentValues = new ContentValues();

        contentValues.put("name", task.getElementContainer().getName());
        contentValues.put("task_id", taskId);

        return contentValues;
    }

    private ContentValues getContentBaseInfo(WSTask task){
        ContentValues contentValues = new ContentValues();

        contentValues.put("name", task.getName());
        contentValues.put("link", task.getLink());

        return contentValues;
    }

    private ContentValues getContentResult(Result result, String task_id){
        ContentValues contentValues = new ContentValues();

        contentValues.put("task_id", task_id);
        contentValues.put("result", result.getResult());

        return contentValues;
    }

    private String getTaskIdByNameAndLink(String taskName , String taskLink){
        Cursor c = db.rawQuery("SELECT id FROM tasks WHERE name='" + taskName + "' AND link='" + taskLink + "'", null);
        int id;
        String s = "";
        while (c.moveToNext()) {
            id = c.getColumnIndex("id");
            s = c.getString(id);
        }
        return s;
    }

    private String getContainerIdByTaskId(String taskId){
        Cursor c = db.rawQuery("SELECT id FROM container_path WHERE task_id=" + taskId, null);
        int id;
        String s = "";
        while (c.moveToNext()) {
            id = c.getColumnIndex("id");
            s = c.getString(id);
        }
        return s;
    }

    private String getDataTagId(){
        Cursor c = db.rawQuery("select id from data_tag ORDER BY id DESC LIMIT 1" , null);
        int id;
        String s = "";
        while (c.moveToNext()) {
            id = c.getColumnIndex("id");
            s = c.getString(id);
        }
        return s;
    }

    private String getElementId(){
        Cursor c = db.rawQuery("select id from element ORDER BY id DESC LIMIT 1" , null);
        int id;
        String s = "";
        while (c.moveToNext()) {
            id = c.getColumnIndex("id");
            s = c.getString(id);
        }
        return s;
    }

    private String getElementPathIdByTaskId(String taskId){
        Cursor c = db.rawQuery("SELECT id FROM element_path WHERE task_id=" + taskId, null);
        int id;
        String s = "";
        while (c.moveToNext()) {
            id = c.getColumnIndex("id");
            s = c.getString(id);
        }
        return s;
    }

    private ContentValues getContentDataContainerPath(String containerPathId, Data data){
        ContentValues contentValues = new ContentValues();

        contentValues.put("container_id", containerPathId);
        contentValues.put("purpose", data.getPurpose());
        contentValues.put("properties", data.getProperties());
        contentValues.put("howSearch", data.getHowSearch());
        contentValues.put("value", data.getValue());

        return contentValues;
    }

    private ContentValues getContentDataElementPath(String containerPathId, Data data){
        ContentValues contentValues = new ContentValues();

        contentValues.put("container_id", containerPathId);
        contentValues.put("purpose", data.getPurpose());
        contentValues.put("properties", data.getProperties());
        contentValues.put("howSearch", data.getHowSearch());
        contentValues.put("value", data.getValue());

        return contentValues;
    }

    private void insertElementsDataTag(String dataTagId, DataTag dataTag) {
        for (Element element: dataTag.getElementList()) {
            ContentValues contentValues = new ContentValues();

            contentValues.put("tag", element.getTag());
            contentValues.put("eLocation", element.geteLocation());
            contentValues.put("eDistance", element.geteDistance());
            contentValues.put("Data_Tag_id", dataTagId);

            long dataTagID = db.insert("element", null, contentValues);
            Log.d(LOG_TAG, "DataTag inserted, ID = " + dataTagID);

            insertDataElement(element, getElementId());
        }
    }

    private void insertDataElement(Element element, String elementId) {
        for (Data data: element.getDataList()) {
            ContentValues contentValues = new ContentValues();

            contentValues.put("container_id", elementId);
            contentValues.put("purpose", data.getPurpose());
            contentValues.put("properties", data.getProperties());
            contentValues.put("howSearch", data.getHowSearch());
            contentValues.put("value", data.getValue());
            contentValues.put("Element_id", elementId);

            long dataElementID = db.insert("data", null, contentValues);
            Log.d(LOG_TAG, "Data element inserted, ID = " + dataElementID);
        }
    }

    ////////////////////////////////////////////////////
    // Get info
    ///////////////////////////////////////////////////

    public List<WSTask> getTasksList(SQLiteDatabase database){
        db = database;

        List<WSTask> wsTaskList = new ArrayList<>();

        Cursor c = db.rawQuery("select * from tasks" , null);

        while (c.moveToNext()) {
            WSTask task = null;
            int taskId = c.getColumnIndex("id");
            int nName = c.getColumnIndex("name");
            int nLink = c.getColumnIndex("link");
            int nContainerPathId = c.getColumnIndex("ContainerPath_id");
            int nElementPathId = c.getColumnIndex("ElementPath_id");

            task = new WSTask(c.getString(nName), c.getString(nLink));

            task.setId(Integer.parseInt(c.getString(taskId)));

            String cpId = c.getString(nContainerPathId);
            String tId = c.getString(taskId);

            task.setContainerStepTwo(getContainerPathById(Integer.parseInt(cpId), Integer.parseInt(tId)));
            task.setElementContainer(getElementPathById(Integer.parseInt(c.getString(nElementPathId)), Integer.parseInt(c.getString(taskId))));
            task.setResultList(getResultByTaskId(Integer.parseInt(c.getString(taskId))));

            wsTaskList.add(task);
        }

        return wsTaskList;
    }

    private List<Result> getResultByTaskId(int taskId){
        List<Result> resultList = new ArrayList<>();

        Cursor c = db.rawQuery("select * from result where task_id=" + taskId, null);

        while (c.moveToNext()) {
            Result result = new Result();
            int resultId = c.getColumnIndex("id");
            int nResult = c.getColumnIndex("result");

            result.setId(Integer.parseInt(c.getString(resultId)));
            result.setResult(c.getString(nResult));
            resultList.add(result);
        }

        return resultList;
    }

    private ElementPath getElementPathById(int elementPathId, int taskId){
        ElementPath elementPath = new ElementPath();

        Cursor c = db.rawQuery("select * from element_path where id='" + elementPathId + "' and " + "task_id='" + taskId + "'" , null);

        while (c.moveToNext()) {
            int elemId = c.getColumnIndex("id");
            int nName = c.getColumnIndex("name");

            elementPath.setId(Integer.parseInt(c.getString(elemId)));
            elementPath.setName(c.getString(nName));
            elementPath.setDataTagList(getDataTagById(Integer.parseInt(c.getString(elemId))));
        }

        return elementPath;
    }

    private List<DataTag> getDataTagById(int elementPathId){
        List<DataTag> list = new ArrayList<>();

        Cursor c = db.rawQuery("select * from data_tag where ElementPath_id=" + elementPathId , null);

        while (c.moveToNext()) {
            DataTag dataTag = new DataTag();

            int dataTagId = c.getColumnIndex("id");
            int nQuantity = c.getColumnIndex("quantity");
            int nNumbeReturneData = c.getColumnIndex("numbeReturneData");
            int nStart = c.getColumnIndex("start");

            dataTag.setId(Integer.parseInt(c.getString(dataTagId)));
            dataTag.setQuantity(Integer.parseInt(c.getString(nQuantity)));
            dataTag.setNumbeReturneData(Integer.parseInt(c.getString(nNumbeReturneData)));
            dataTag.setStart(c.getString(nStart));
            dataTag.setElementList(getElementByDataTagId(Integer.parseInt(c.getString(dataTagId))));

            list.add(dataTag);
        }

        return list;
    }

    private List<Element> getElementByDataTagId(int dataTagId){
        List<Element> elementList = new ArrayList<>();

        Cursor c = db.rawQuery("select * from element where Data_Tag_id=" + dataTagId , null);

        while (c.moveToNext()) {
            Element element = new Element();

            int elementId = c.getColumnIndex("id");
            int nTag = c.getColumnIndex("tag");
            int nElocation = c.getColumnIndex("eLocation");
            int nEDistance = c.getColumnIndex("eDistance");

            element.setId(Integer.parseInt(c.getString(elementId)));
            element.setTag(c.getString(nTag));
            element.seteLocation(c.getString(nElocation));
            element.seteDistance(c.getString(nEDistance));
            element.setDataList(getDataById(dataTagId,Integer.parseInt(c.getString(elementId))));

            elementList.add(element);
        }

        return elementList;
    }

    private ContainerPath getContainerPathById(int containerPathId, int taskId){
        ContainerPath containerPath = new ContainerPath();

        Cursor c = db.rawQuery("select * from container_path where id='" + containerPathId + "' and " + "task_id='" + taskId + "'" , null);

        while (c.moveToNext()) {
            int contPathId = c.getColumnIndex("id");
            int nTag = c.getColumnIndex("tag");
            int nELocation = c.getColumnIndex("eLocation");
            int nEDistance = c.getColumnIndex("eDistance");

            containerPath.setId(Integer.parseInt(c.getString(contPathId)));
            containerPath.setTag(c.getString(nTag));
            containerPath.setWhere(c.getString(nELocation));
            containerPath.setDistance(c.getString(nEDistance));
            containerPath.setDataList(getDataById(Integer.parseInt(c.getString(contPathId))));
        }

        return containerPath;
    }

    private List<Data> getDataById(int containerId){
        List<Data> list = new ArrayList<>();

        Cursor c = db.rawQuery("select * from data where container_id=" + containerId , null);

        while (c.moveToNext()) {
            Data data = new Data();

            int dataId = c.getColumnIndex("id");
            int nPurpose = c.getColumnIndex("purpose");
            int nProperties = c.getColumnIndex("properties");
            int nHowSearch = c.getColumnIndex("howSearch");
            int nValue = c.getColumnIndex("value");

            data.setId(Integer.parseInt(c.getString(dataId)));
            data.setPurpose(c.getString(nPurpose));
            data.setProperties(c.getString(nProperties));
            data.setHowSearch(c.getString(nHowSearch));
            data.setValue(c.getString(nValue));

            list.add(data);
        }

        return list;
    }

    private List<Data> getDataById(int containerId, int elementId){
        List<Data> list = new ArrayList<>();

        Cursor c = db.rawQuery("select * from data where container_id='" + containerId + "' and Element_id='" + elementId + "'", null);

        while (c.moveToNext()) {
            Data data = new Data();

            int dataId = c.getColumnIndex("id");
            int nPurpose = c.getColumnIndex("purpose");
            int nProperties = c.getColumnIndex("properties");
            int nHowSearch = c.getColumnIndex("howSearch");
            int nValue = c.getColumnIndex("value");

            data.setId(Integer.parseInt(c.getString(dataId)));
            data.setPurpose(c.getString(nPurpose));
            data.setProperties(c.getString(nProperties));
            data.setHowSearch(c.getString(nHowSearch));
            data.setValue(c.getString(nValue));

            list.add(data);
        }

        return list;
    }

    ////////////////////////////////////////////////////
    // Update info
    ///////////////////////////////////////////////////

    private ContentValues getUpdateContentBaseInfo(WSTask task, int containerPathId, int elementPathId){
        ContentValues contentValues = new ContentValues();

        /*contentValues.put("name", task.getName());
        contentValues.put("link", task.getLink());*/
        contentValues.put("ContainerPath_id", containerPathId);
        contentValues.put("ElementPath_id", elementPathId);

        return contentValues;
    }
}
