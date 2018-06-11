package com.vottakvot.nevskiy.knuckles.Parser.synchronize;

import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;

import com.vottakvot.nevskiy.knuckles.Parser.WSTask;
import com.vottakvot.nevskiy.knuckles.Parser.data.ContainerPath;
import com.vottakvot.nevskiy.knuckles.Parser.data.Data;
import com.vottakvot.nevskiy.knuckles.Parser.data.DataTag;
import com.vottakvot.nevskiy.knuckles.Parser.data.Element;
import com.vottakvot.nevskiy.knuckles.Parser.data.ElementPath;
import com.vottakvot.nevskiy.knuckles.Parser.data.Result;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Shuyning on 10.06.2018.
 */

public class Synchro {
    private SQLiteDatabase db;
    private List<WSTask> wsTaskList;

    ArrayList<String> dtaTagElement = null;
    ArrayList<Integer> resultsElement = null;

    public Synchro(SQLiteDatabase database) {
        db = database;
        wsTaskList = getTasksList(db);
        System.out.println();
    }

    public ArrayList<String> getDtaTagElement() {
        return dtaTagElement;
    }

    public List<WSTask> getWsTaskList() {
        return wsTaskList;
    }

    public void setWsTaskList(List<WSTask> wsTaskList) {
        this.wsTaskList = wsTaskList;
    }

    public ArrayList<Integer> getResultsElement() {
        return resultsElement;
    }

    public void setResultsElement(ArrayList<Integer> resultsElement) {
        this.resultsElement = resultsElement;
    }

    public void setDtaTagElement(ArrayList<String> dtaTagElement) {
        this.dtaTagElement = dtaTagElement;
    }

    public List<WSTask> getTasksList(SQLiteDatabase database) {
        db = database;
        ArrayList<Integer> pathways = null;
        ArrayList<Integer> elements = null;
        ArrayList<Integer> results = null;
        ArrayList<String> dataTag = null;

        List<WSTask> wsTaskList = new ArrayList<>();

        Cursor c = db.rawQuery("select * from tasks", null);

        while (c.moveToNext()) {
            WSTask task;

            int taskId = c.getColumnIndex("id");
            int nName = c.getColumnIndex("name");
            int nLink = c.getColumnIndex("link");
            int nContainerPathId = c.getColumnIndex("ContainerEl_id");
            int nElementPathId = c.getColumnIndex("ElementPath_id");

            task = new WSTask(c.getString(nName), c.getString(nLink));

            task.setId(Integer.parseInt(c.getString(taskId)));

            String tagContainerTwo = getElementStepTwoTag(c.getString(nContainerPathId));
            task.getContainerStepTwo().setTag(tagContainerTwo);
            task.getContainerStepTwo().setDistance("1");
            task.getContainerStepTwo().setWhere("внутри");

            setDataStepTwoTag(c.getString(nContainerPathId), task.getContainerStepTwo());

            pathways = getArrayElementPathway(c.getString(taskId));
            elements = getArrayElement(pathways);
            dataTag = getDtaTagElement();

            task.getElementContainer().setDataTagList(getDataTagById(pathways, elements, dataTag));
            results = resultsElement;

            task.setResultList(getResultByTaskId(results));

            /*ask.setContainerStepTwo(getContainerPathById(Integer.parseInt(cpId), Integer.parseInt(tId)));
            task.setElementContainer(getElementPathById(Integer.parseInt(c.getString(nElementPathId)), Integer.parseInt(c.getString(taskId))));
            task.setResultResp(getResultByTaskId(Integer.parseInt(c.getString(taskId))));*/

            wsTaskList.add(task);
        }

        return wsTaskList;
    }


    private ArrayList<Integer> getArrayElementPathway(String taskId) {

        Cursor c = db.rawQuery("select * from pathways where task_id=" + taskId, null);
        ArrayList<Integer> integers = new ArrayList<>();

        while (c.moveToNext()) {
            int tag = c.getColumnIndex("id");
            integers.add(c.getInt(tag));
        }

        return integers;
    }

    private ArrayList<Integer> getArrayElement(ArrayList<Integer> pathways) {
        ArrayList<Integer> integers = new ArrayList<>();
        ArrayList<String> dataTags = new ArrayList<>();

        for (Integer integer : pathways) {
            Cursor c = db.rawQuery("select * from elements where pathway_id=" + integer, null);

            while (c.moveToNext()) {
                int id = c.getColumnIndex("id");
                int tag = c.getColumnIndex("tag");
                integers.add(c.getInt(id));
                dataTags.add(c.getString(tag));
            }
        }

        setDtaTagElement(dataTags);
        return integers;
    }

    private String getElementStepTwoTag(String taskId) {

        Cursor c = db.rawQuery("select * from elements where id=" + taskId, null);
        String str = null;

        while (c.moveToNext()) {
            int tag = c.getColumnIndex("tag");

            str = c.getString(tag);
        }

        return str;
    }

    private void setDataStepTwoTag(String contId, ContainerPath containerPath) {

        Cursor c = db.rawQuery("select * from data where element_id=" + contId, null);
        String str = null;
        List<Data> dataList = new ArrayList<>();

        while (c.moveToNext()) {
            Data data = new Data();

            int prop = c.getColumnIndex("prop");
            int value = c.getColumnIndex("value");

            data.setProperties(c.getString(prop));
            data.setValue(c.getString(value));

            data.setPurpose("1");
            data.setHowSearch("1");

            dataList.add(data);
        }

        containerPath.setDataList(dataList);
    }

    private List<Result> getResultByTaskId(ArrayList<Integer> dataId) {
        List<Result> resultList = new ArrayList<>();
        String[] results = new String[dataId.size() * 3];
        for (int a = 0; a < results.length; a++) {
            results[a] = "";
        }
        int i = 0;

        for (Integer integer : dataId) {
            Cursor c = db.rawQuery("select * from records where data_id=" + integer, null);

            while (c.moveToNext()) {
                int text = c.getColumnIndex("text");

                results[i] += c.getString(text) + "\n";
                i++;
            }
            i = 0;
        }

        for (String s: results) {
            if (s != null && !s.equals("")) {
                Result result = new Result();
                result.setResult(s);
                resultList.add(result);
            }
        }

        return resultList;
    }

    private ElementPath getElementPathById(int elementPathId, int taskId) {
        ElementPath elementPath = new ElementPath();

        Cursor c = db.rawQuery("select * from element_path where id='" + elementPathId + "' and " + "task_id='" + taskId + "'", null);

        while (c.moveToNext()) {
            int elemId = c.getColumnIndex("id");
            int nName = c.getColumnIndex("name");

            elementPath.setId(Integer.parseInt(c.getString(elemId)));
            elementPath.setName(c.getString(nName));
            //elementPath.setDataTagList(getDataTagById(Integer.parseInt(c.getString(elemId))));
        }

        return elementPath;
    }

    private List<DataTag> getDataTagById(ArrayList<Integer> pathways, ArrayList<Integer> elements, ArrayList<String> dataTag) {
        List<DataTag> list = new ArrayList<>();
        ArrayList<Integer> resEl = new ArrayList<>();

        for (int i = 0; i < elements.size(); i++) {
            Cursor c = db.rawQuery("select * from data where element_id=" + elements.get(i), null);
            List<Element> elementList = new ArrayList<>();
            List<Data> dataList = new ArrayList<>();

            DataTag dataT = new DataTag();
            dataT.setNumbeReturneData(1);
            dataT.setQuantity(1);
            dataT.setStart("start");

            Element element = new Element();
            element.setTag(dataTag.get(i));
            element.seteLocation("внутри");
            element.seteDistance("1");

            while (c.moveToNext()) {
                Data data = new Data();

                int prop = c.getColumnIndex("prop");
                int value = c.getColumnIndex("value");

                int id = c.getColumnIndex("id");

                resEl.add(c.getInt(id));

                data.setProperties(c.getString(prop));
                data.setValue(c.getString(value) + "");
                data.setPurpose("1");
                data.setHowSearch("1");

                dataList.add(data);
            }
            element.setDataList(dataList);
            elementList.add(element);
            dataT.setElementList(elementList);
            list.add(dataT);
        }

        setResultsElement(resEl);

        return list;
    }

    private List<Element> getElementByDataTagId(int dataTagId) {
        List<Element> elementList = new ArrayList<>();

        Cursor c = db.rawQuery("select * from element where Data_Tag_id=" + dataTagId, null);

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
            element.setDataList(getDataById(dataTagId, Integer.parseInt(c.getString(elementId))));

            elementList.add(element);
        }

        return elementList;
    }

    private ContainerPath getContainerPathById(int containerPathId, int taskId) {
        ContainerPath containerPath = new ContainerPath();

        Cursor c = db.rawQuery("select * from container_path where id='" + containerPathId + "' and " + "task_id='" + taskId + "'", null);

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

    private List<Data> getDataById(int containerId) {
        List<Data> list = new ArrayList<>();

        Cursor c = db.rawQuery("select * from data where container_id=" + containerId, null);

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

    private List<Data> getDataById(int containerId, int elementId) {
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
}
