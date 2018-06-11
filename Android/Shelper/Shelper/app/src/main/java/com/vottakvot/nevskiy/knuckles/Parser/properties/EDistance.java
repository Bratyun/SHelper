package com.vottakvot.nevskiy.knuckles.Parser.properties;

/**
 * Created by Shuyning on 03.06.2018.
 */

public enum EDistance {
    FIRST("first");

    private String value;

    EDistance(String value) {
        this.value = value;
    }

    public boolean equalsTo(String name) {
        return value.equals(name);
    }

    public String value() {
        return value;
    }
}
