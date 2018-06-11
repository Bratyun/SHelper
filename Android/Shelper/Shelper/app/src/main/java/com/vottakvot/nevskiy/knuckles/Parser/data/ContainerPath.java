package com.vottakvot.nevskiy.knuckles.Parser.data;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Shuyning on 03.06.2018.
 */

public class ContainerPath {
    private int id;
    private String tag;
    private List<Data> dataList;

    private String eLocation;
    private String eDistance;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getWhere() {
        return eLocation;
    }

    public void setWhere(String where) {
        this.eLocation = where;
    }

    public String getDistance() {
        return eDistance;
    }

    public void setDistance(String distance) {
        this.eDistance = distance;
    }

    public String getTag() {
        return tag;
    }

    public void setTag(String tag) {
        this.tag = tag;
    }

    public List<Data> getDataList() {
        return dataList;
    }

    public void setDataList(List<Data> dataList) {
        this.dataList = dataList;
    }

    public void addData(String purpose, String properties, String howSearch, String value) {
        Data data = new Data();

        if (dataList == null) {
            dataList = new ArrayList<>();
        }

        if (purpose != null) {
            data.setPurpose(purpose);
        }
        if (properties != null) {
            data.setProperties(properties);
        }
        if (howSearch != null) {
            data.setHowSearch(howSearch);
        }
        if (value != null) {
            data.setValue(value);
        }

        dataList.add(data);
    }
}
