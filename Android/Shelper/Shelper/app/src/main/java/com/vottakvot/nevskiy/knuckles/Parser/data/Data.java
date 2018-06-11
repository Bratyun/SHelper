package com.vottakvot.nevskiy.knuckles.Parser.data;



/**
 * Created by Shuyning on 03.06.2018.
 */

public class Data {
    private int id;
    private String purpose;
    private String properties;
    private String howSearch;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    private String value;

    public String getPurpose() {
        return purpose;
    }

    public void setPurpose(String purpose) {
        this.purpose = purpose;
    }

    public String getProperties() {
        return properties;
    }

    public void setProperties(String properties) {
        this.properties = properties;
    }

    public String getHowSearch() {
        return howSearch;
    }

    public void setHowSearch(String howSearch) {
        this.howSearch = howSearch;
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }
}
