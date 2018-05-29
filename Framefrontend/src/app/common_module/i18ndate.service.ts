//日期多语言管理
import { NgModule } from '@angular/core';
import { Injectable } from '@angular/core';

@Injectable()
export class I18nDateService {

    constructor() { }

    public  getCurrDateLocalConfig(currlang: string): any {
        let resObj:any;
        if(currlang == "zh-CN"){
            resObj = this.China_date;
        }
        else if(currlang == "en-ww"){
            resObj = this.Eng_date;
        }
        return resObj;
    }

    //中文配置
    private China_date = {
        firstDayOfWeek: 0,
        dayNames: ['星期一', '星期二', '星期三', '星期四', '星期五', '星期六', '星期日'],
        dayNamesShort: ['一', '二', '三', '四', '五', '六', '日'],
        dayNamesMin: ['一', '二', '三', '四', '五', '六', '日'],
        monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
        monthNamesShort: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
        today: '今日',
        clear: '清空'
    };

    //英文配置
    private Eng_date = {
        firstDayOfWeek: 0,
        dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
        dayNamesShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
        dayNamesMin: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
        monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
        monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
        today: 'Today',
        clear: 'Clear'
    };

    //后续扩展其他语言
}