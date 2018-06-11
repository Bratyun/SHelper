package com.vottakvot.nevskiy.knuckles.Parser.data;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Shuyning on 06.06.2018.
 */

public class Element {
    private int id;

    private String tag;

    private String eLocation;
    private String eDistance;

    private List<Data> dataList = new ArrayList<>();

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public void addDataList(Data data){
        if (data != null){
            dataList.add(data);
        }
    }

    public String getTag() {
        return tag;
    }

    public void setTag(String tag) {
        this.tag = tag;
    }

    public String geteLocation() {
        return eLocation;
    }

    public void seteLocation(String eLocation) {
        this.eLocation = eLocation;
    }

    public String geteDistance() {
        return eDistance;
    }

    public void seteDistance(String eDistance) {
        this.eDistance = eDistance;
    }

    public List<Data> getDataList() {
        return dataList;
    }

    public void setDataList(List<Data> dataList) {
        this.dataList = dataList;
    }
}
