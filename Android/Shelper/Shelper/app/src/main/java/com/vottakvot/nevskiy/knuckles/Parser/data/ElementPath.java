package com.vottakvot.nevskiy.knuckles.Parser.data;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Shuyning on 06.06.2018.
 */

public class ElementPath {
    private int id;
    private String name;

    private List<DataTag> dataTagList;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public ElementPath() {
        this.dataTagList = new ArrayList<>();
    }

    public List<DataTag> getDataTagList() {
        return dataTagList;
    }

    public void setDataTagList(List<DataTag> dataTagList) {
        this.dataTagList = dataTagList;
    }
}
