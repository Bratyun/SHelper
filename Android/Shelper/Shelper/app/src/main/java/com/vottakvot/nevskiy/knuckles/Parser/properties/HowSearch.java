package com.vottakvot.nevskiy.knuckles.Parser.properties;

/**
 * Created by Shuyning on 03.06.2018.
 */

public enum HowSearch {
    INSIDE("inside");

    private String value;

    HowSearch(String value) {
        this.value = value;
    }

    public boolean equalsTo(String name) {
        return value.equals(name);
    }

    public String value() {
        return value;
    }
}
