using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Asset;
namespace Framework.Tables {
    public class TableSound
    {
        private static string FileName = "Table/Sound";
        private static bool Inited = false;

        public TableSound()
		{
		}
        public void Clear()
        {
            Inited = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private string _anwser_right;
        public string anwser_right
        {
            private set
            {
                _anwser_right = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _anwser_right;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string _anwser_wrong;
        public string anwser_wrong
        {
            private set
            {
                _anwser_wrong = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _anwser_wrong;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string _click;
        public string click
        {
            private set
            {
                _click = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _click;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string _explain_show;
        public string explain_show
        {
            private set
            {
                _explain_show = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _explain_show;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string _next_level;
        public string next_level
        {
            private set
            {
                _next_level = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _next_level;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string _upgrade;
        public string upgrade
        {
            private set
            {
                _upgrade = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _upgrade;
            }
        }
        /// <summary>
        /// 通用按钮
        /// </summary>
        private string _sfx_common_btn;
        public string sfx_common_btn
        {
            private set
            {
                _sfx_common_btn = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_btn;
            }
        }
        /// <summary>
        /// 通用界面弹出
        /// </summary>
        private string _sfx_common_panel_show;
        public string sfx_common_panel_show
        {
            private set
            {
                _sfx_common_panel_show = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_panel_show;
            }
        }
        /// <summary>
        /// 特殊界面弹出
        /// </summary>
        private string _sfx_special_panel_show;
        public string sfx_special_panel_show
        {
            private set
            {
                _sfx_special_panel_show = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_special_panel_show;
            }
        }
        /// <summary>
        /// 通用界面关闭
        /// </summary>
        private string _sfx_common_panel_close;
        public string sfx_common_panel_close
        {
            private set
            {
                _sfx_common_panel_close = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_panel_close;
            }
        }
        /// <summary>
        /// 通用礼盒出现
        /// </summary>
        private string _sfx_common_gift_show;
        public string sfx_common_gift_show
        {
            private set
            {
                _sfx_common_gift_show = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_gift_show;
            }
        }
        /// <summary>
        /// 通用礼盒未点击停留
        /// </summary>
        private string _sfx_common_gift_wait;
        public string sfx_common_gift_wait
        {
            private set
            {
                _sfx_common_gift_wait = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_gift_wait;
            }
        }
        /// <summary>
        /// 通用礼盒打开
        /// </summary>
        private string _sfx_common_gift_open;
        public string sfx_common_gift_open
        {
            private set
            {
                _sfx_common_gift_open = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_gift_open;
            }
        }
        /// <summary>
        /// 星星飞行音效
        /// </summary>
        private string _sfx_common_starfly;
        public string sfx_common_starfly
        {
            private set
            {
                _sfx_common_starfly = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_starfly;
            }
        }
        /// <summary>
        /// 任务星星-1音效
        /// </summary>
        private string _sfx_common_starbeused;
        public string sfx_common_starbeused
        {
            private set
            {
                _sfx_common_starbeused = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_starbeused;
            }
        }
        /// <summary>
        /// 任务完成并消失音效
        /// </summary>
        private string _sfx_common_taskover;
        public string sfx_common_taskover
        {
            private set
            {
                _sfx_common_taskover = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_taskover;
            }
        }
        /// <summary>
        /// 新任务出现音效
        /// </summary>
        private string _sfx_common_taskappear;
        public string sfx_common_taskappear
        {
            private set
            {
                _sfx_common_taskappear = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_taskappear;
            }
        }
        /// <summary>
        /// 新任务弹板出现音效
        /// </summary>
        private string _sfx_newtask_open;
        public string sfx_newtask_open
        {
            private set
            {
                _sfx_newtask_open = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_newtask_open;
            }
        }
        /// <summary>
        /// 新任务弹板消失音效
        /// </summary>
        private string _sfx_newtask_close;
        public string sfx_newtask_close
        {
            private set
            {
                _sfx_newtask_close = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_newtask_close;
            }
        }
        /// <summary>
        /// 对话框文字显示音效
        /// </summary>
        private string _sfx_word_show;
        public string sfx_word_show
        {
            private set
            {
                _sfx_word_show = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_word_show;
            }
        }
        /// <summary>
        /// 随机剧情气泡出现音效
        /// </summary>
        private string _sfx_bubble;
        public string sfx_bubble
        {
            private set
            {
                _sfx_bubble = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_bubble;
            }
        }
        /// <summary>
        /// 房间修复完成
        /// </summary>
        private string _sfx_room_repair;
        public string sfx_room_repair
        {
            private set
            {
                _sfx_room_repair = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_room_repair;
            }
        }
        /// <summary>
        /// 新的一天
        /// </summary>
        private string _sfx_newday;
        public string sfx_newday
        {
            private set
            {
                _sfx_newday = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_newday;
            }
        }
        /// <summary>
        /// 照片列表打开、关闭照片
        /// </summary>
        private string _sfx_photo;
        public string sfx_photo
        {
            private set
            {
                _sfx_photo = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_photo;
            }
        }
        /// <summary>
        /// 普通转盘转动
        /// </summary>
        private string _sfx_turntable01;
        public string sfx_turntable01
        {
            private set
            {
                _sfx_turntable01 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_turntable01;
            }
        }
        /// <summary>
        /// 报纸转盘转动
        /// </summary>
        private string _sfx_turntable02;
        public string sfx_turntable02
        {
            private set
            {
                _sfx_turntable02 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_turntable02;
            }
        }
        /// <summary>
        /// 通用得到金币
        /// </summary>
        private string _sfx_common_getcoins;
        public string sfx_common_getcoins
        {
            private set
            {
                _sfx_common_getcoins = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_getcoins;
            }
        }
        /// <summary>
        /// 开始界面背景音乐
        /// </summary>
        private string _BGM_daily;
        public string BGM_daily
        {
            private set
            {
                _BGM_daily = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _BGM_daily;
            }
        }
        /// <summary>
        /// 开始界面logo、开始按钮出现音效
        /// </summary>
        private string _sfx_logo;
        public string sfx_logo
        {
            private set
            {
                _sfx_logo = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_logo;
            }
        }
        /// <summary>
        /// 游戏开始按钮
        /// </summary>
        private string _sfx_btn_normal;
        public string sfx_btn_normal
        {
            private set
            {
                _sfx_btn_normal = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_btn_normal;
            }
        }
        /// <summary>
        /// 邮件打开音效
        /// </summary>
        private string _sfx_letter;
        public string sfx_letter
        {
            private set
            {
                _sfx_letter = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_letter;
            }
        }
        /// <summary>
        /// 长按家具换肤音效
        /// </summary>
        private string _sfx_longpress;
        public string sfx_longpress
        {
            private set
            {
                _sfx_longpress = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_longpress;
            }
        }
        /// <summary>
        /// 进关动画音效
        /// </summary>
        private string _sfx_transition;
        public string sfx_transition
        {
            private set
            {
                _sfx_transition = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_transition;
            }
        }
        /// <summary>
        /// 背景音乐（外围）
        /// </summary>
        private string _BGM_home;
        public string BGM_home
        {
            private set
            {
                _BGM_home = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _BGM_home;
            }
        }
        /// <summary>
        /// 章节进度条上涨
        /// </summary>
        private string _sfx_progressincrease;
        public string sfx_progressincrease
        {
            private set
            {
                _sfx_progressincrease = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_progressincrease;
            }
        }
        /// <summary>
        /// PLAY按钮
        /// </summary>
        private string _sfx_entergame;
        public string sfx_entergame
        {
            private set
            {
                _sfx_entergame = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_entergame;
            }
        }
        /// <summary>
        /// 页签切换
        /// </summary>
        private string _sfx_pageturn;
        public string sfx_pageturn
        {
            private set
            {
                _sfx_pageturn = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_pageturn;
            }
        }
        /// <summary>
        /// 背景音乐（关内）
        /// </summary>
        private string _BGM_play1;
        public string BGM_play1
        {
            private set
            {
                _BGM_play1 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _BGM_play1;
            }
        }
        /// <summary>
        /// combo+1
        /// </summary>
        private string _sfx_combo_good;
        public string sfx_combo_good
        {
            private set
            {
                _sfx_combo_good = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_combo_good;
            }
        }
        /// <summary>
        /// combo+1人声
        /// </summary>
        private string _sfx_combo_good_voice;
        public string sfx_combo_good_voice
        {
            private set
            {
                _sfx_combo_good_voice = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_combo_good_voice;
            }
        }
        /// <summary>
        /// combo+2
        /// </summary>
        private string _sfx_combo_great;
        public string sfx_combo_great
        {
            private set
            {
                _sfx_combo_great = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_combo_great;
            }
        }
        /// <summary>
        /// combo+2人声
        /// </summary>
        private string _sfx_combo_great_voice;
        public string sfx_combo_great_voice
        {
            private set
            {
                _sfx_combo_great_voice = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_combo_great_voice;
            }
        }
        /// <summary>
        /// combo+3
        /// </summary>
        private string _sfx_combo_excellent;
        public string sfx_combo_excellent
        {
            private set
            {
                _sfx_combo_excellent = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_combo_excellent;
            }
        }
        /// <summary>
        /// combo+3人声
        /// </summary>
        private string _sfx_combo_excellent_voice;
        public string sfx_combo_excellent_voice
        {
            private set
            {
                _sfx_combo_excellent_voice = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_combo_excellent_voice;
            }
        }
        /// <summary>
        /// combo+4
        /// </summary>
        private string _sfx_combo_amazing;
        public string sfx_combo_amazing
        {
            private set
            {
                _sfx_combo_amazing = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_combo_amazing;
            }
        }
        /// <summary>
        /// combo+4人声
        /// </summary>
        private string _sfx_combo_amazing_voice;
        public string sfx_combo_amazing_voice
        {
            private set
            {
                _sfx_combo_amazing_voice = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_combo_amazing_voice;
            }
        }
        /// <summary>
        /// combo+5
        /// </summary>
        private string _sfx_combo_outstanding;
        public string sfx_combo_outstanding
        {
            private set
            {
                _sfx_combo_outstanding = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_combo_outstanding;
            }
        }
        /// <summary>
        /// combo+5人声
        /// </summary>
        private string _sfx_combo_outstanding_voice;
        public string sfx_combo_outstanding_voice
        {
            private set
            {
                _sfx_combo_outstanding_voice = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_combo_outstanding_voice;
            }
        }
        /// <summary>
        /// combo+6
        /// </summary>
        private string _sfx_combo_unbelivable;
        public string sfx_combo_unbelivable
        {
            private set
            {
                _sfx_combo_unbelivable = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_combo_unbelivable;
            }
        }
        /// <summary>
        /// combo+6人声
        /// </summary>
        private string _sfx_combo_unbelivable_voice;
        public string sfx_combo_unbelivable_voice
        {
            private set
            {
                _sfx_combo_unbelivable_voice = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_combo_unbelivable_voice;
            }
        }
        /// <summary>
        /// 划词加分音效
        /// </summary>
        private string _sfx_score;
        public string sfx_score
        {
            private set
            {
                _sfx_score = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_score;
            }
        }
        /// <summary>
        /// 词盘滑动字母第一个
        /// </summary>
        private string _sfx_click_letter_1;
        public string sfx_click_letter_1
        {
            private set
            {
                _sfx_click_letter_1 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_click_letter_1;
            }
        }
        /// <summary>
        /// 词盘滑动字母第二个
        /// </summary>
        private string _sfx_click_letter_2;
        public string sfx_click_letter_2
        {
            private set
            {
                _sfx_click_letter_2 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_click_letter_2;
            }
        }
        /// <summary>
        /// 词盘滑动字母第三个
        /// </summary>
        private string _sfx_click_letter_3;
        public string sfx_click_letter_3
        {
            private set
            {
                _sfx_click_letter_3 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_click_letter_3;
            }
        }
        /// <summary>
        /// 词盘滑动字母第四个
        /// </summary>
        private string _sfx_click_letter_4;
        public string sfx_click_letter_4
        {
            private set
            {
                _sfx_click_letter_4 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_click_letter_4;
            }
        }
        /// <summary>
        /// 词盘滑动字母第五个
        /// </summary>
        private string _sfx_click_letter_5;
        public string sfx_click_letter_5
        {
            private set
            {
                _sfx_click_letter_5 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_click_letter_5;
            }
        }
        /// <summary>
        /// 词盘滑动字母第六个
        /// </summary>
        private string _sfx_click_letter_6;
        public string sfx_click_letter_6
        {
            private set
            {
                _sfx_click_letter_6 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_click_letter_6;
            }
        }
        /// <summary>
        /// 词盘滑动字母第七个
        /// </summary>
        private string _sfx_click_letter_7;
        public string sfx_click_letter_7
        {
            private set
            {
                _sfx_click_letter_7 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_click_letter_7;
            }
        }
        /// <summary>
        /// shuffle（字母换位）
        /// </summary>
        private string _sfx_shuffle;
        public string sfx_shuffle
        {
            private set
            {
                _sfx_shuffle = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_shuffle;
            }
        }
        /// <summary>
        /// 进入关卡金币词
        /// </summary>
        private string _sfx_coinword;
        public string sfx_coinword
        {
            private set
            {
                _sfx_coinword = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_coinword;
            }
        }
        /// <summary>
        /// 猜对词
        /// </summary>
        private string _sfx_match;
        public string sfx_match
        {
            private set
            {
                _sfx_match = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_match;
            }
        }
        /// <summary>
        /// 猜错词
        /// </summary>
        private string _sfx_wrong;
        public string sfx_wrong
        {
            private set
            {
                _sfx_wrong = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_wrong;
            }
        }
        /// <summary>
        /// 猜到重复词
        /// </summary>
        private string _sfx_repeat;
        public string sfx_repeat
        {
            private set
            {
                _sfx_repeat = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_repeat;
            }
        }
        /// <summary>
        /// 额外词音效
        /// </summary>
        private string _sfx_bouns_win;
        public string sfx_bouns_win
        {
            private set
            {
                _sfx_bouns_win = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_bouns_win;
            }
        }
        /// <summary>
        /// 额外词重复词
        /// </summary>
        private string _sfx_bouns_repeat;
        public string sfx_bouns_repeat
        {
            private set
            {
                _sfx_bouns_repeat = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_bouns_repeat;
            }
        }
        /// <summary>
        /// 额外词飞向bouns盒子
        /// </summary>
        private string _sfx_addScore;
        public string sfx_addScore
        {
            private set
            {
                _sfx_addScore = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_addScore;
            }
        }
        /// <summary>
        /// 最后1词提示音效
        /// </summary>
        private string _sfx_lastword;
        public string sfx_lastword
        {
            private set
            {
                _sfx_lastword = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_lastword;
            }
        }
        /// <summary>
        /// hint按钮音效
        /// </summary>
        private string _sfx_subworld_open;
        public string sfx_subworld_open
        {
            private set
            {
                _sfx_subworld_open = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_subworld_open;
            }
        }
        /// <summary>
        /// hint1填充音效
        /// </summary>
        private string _sfx_usehint;
        public string sfx_usehint
        {
            private set
            {
                _sfx_usehint = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_usehint;
            }
        }
        /// <summary>
        /// hint2选中效果
        /// </summary>
        private string _sfx_specific_hint_click;
        public string sfx_specific_hint_click
        {
            private set
            {
                _sfx_specific_hint_click = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_specific_hint_click;
            }
        }
        /// <summary>
        /// hint2填充音效
        /// </summary>
        private string _sfx_specific_hint;
        public string sfx_specific_hint
        {
            private set
            {
                _sfx_specific_hint = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_specific_hint;
            }
        }
        /// <summary>
        /// hint3填充音效(闪电）
        /// </summary>
        private string _sfx_mult_hint;
        public string sfx_mult_hint
        {
            private set
            {
                _sfx_mult_hint = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_mult_hint;
            }
        }
        /// <summary>
        /// 猜对金币词
        /// </summary>
        private string _sfx_extra_collect;
        public string sfx_extra_collect
        {
            private set
            {
                _sfx_extra_collect = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_extra_collect;
            }
        }
        /// <summary>
        /// 结算界面出现
        /// </summary>
        private string _sfx_level_win;
        public string sfx_level_win
        {
            private set
            {
                _sfx_level_win = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_level_win;
            }
        }
        /// <summary>
        /// 结算界面待机
        /// </summary>
        private string _sfx_level_wait;
        public string sfx_level_wait
        {
            private set
            {
                _sfx_level_wait = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_level_wait;
            }
        }
        /// <summary>
        /// 结算界面获取星星
        /// </summary>
        private string _sfx_level_getstar;
        public string sfx_level_getstar
        {
            private set
            {
                _sfx_level_getstar = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_level_getstar;
            }
        }
        /// <summary>
        /// 得到花瓣
        /// </summary>
        private string _sfx_win_effect;
        public string sfx_win_effect
        {
            private set
            {
                _sfx_win_effect = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_win_effect;
            }
        }
        /// <summary>
        /// 花瓣停止
        /// </summary>
        private string _sfx_win_stop;
        public string sfx_win_stop
        {
            private set
            {
                _sfx_win_stop = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_win_stop;
            }
        }
        /// <summary>
        /// 得到挑战券
        /// </summary>
        private string _sfx_starfall1;
        public string sfx_starfall1
        {
            private set
            {
                _sfx_starfall1 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_starfall1;
            }
        }
        /// <summary>
        /// 每日挑战背景音乐
        /// </summary>
        private string _BGM_play_daily;
        public string BGM_play_daily
        {
            private set
            {
                _BGM_play_daily = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _BGM_play_daily;
            }
        }
        /// <summary>
        /// 每日挑战花出现
        /// </summary>
        private string _sfx_dailyreward_flower;
        public string sfx_dailyreward_flower
        {
            private set
            {
                _sfx_dailyreward_flower = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_dailyreward_flower;
            }
        }
        /// <summary>
        /// 每日挑战收集花
        /// </summary>
        private string _sfx_dailyreward_gift;
        public string sfx_dailyreward_gift
        {
            private set
            {
                _sfx_dailyreward_gift = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_dailyreward_gift;
            }
        }
        /// <summary>
        /// 每日挑战获得星星
        /// </summary>
        private string _sfx_daily_star_unlock;
        public string sfx_daily_star_unlock
        {
            private set
            {
                _sfx_daily_star_unlock = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_daily_star_unlock;
            }
        }
        /// <summary>
        /// 每日挑战获得3星
        /// </summary>
        private string _sfx_dailyreward_open;
        public string sfx_dailyreward_open
        {
            private set
            {
                _sfx_dailyreward_open = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_dailyreward_open;
            }
        }
        /// <summary>
        /// 结算界面出现
        /// </summary>
        private string _sfx_complete_change;
        public string sfx_complete_change
        {
            private set
            {
                _sfx_complete_change = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_complete_change;
            }
        }
        /// <summary>
        /// 背景音乐（关内）
        /// </summary>
        private string _BGM_play2;
        public string BGM_play2
        {
            private set
            {
                _BGM_play2 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _BGM_play2;
            }
        }
        /// <summary>
        /// 背景音乐（关内）
        /// </summary>
        private string _BGM_play3;
        public string BGM_play3
        {
            private set
            {
                _BGM_play3 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _BGM_play3;
            }
        }
        /// <summary>
        /// 背景音乐（关内）
        /// </summary>
        private string _BGM_play4;
        public string BGM_play4
        {
            private set
            {
                _BGM_play4 = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _BGM_play4;
            }
        }
        /// <summary>
        /// 通用消耗星星
        /// </summary>
        private string _sfx_common_star;
        public string sfx_common_star
        {
            private set
            {
                _sfx_common_star = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_star;
            }
        }


        public void Init () {
            if (!Table.Inited)
            {
                throw new Exception("Tabele has not been initialised,it is intialised in GameController");
            }
            if (!Inited) {
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode itemData = JSONNode.Parse (tableStr);
				anwser_right = itemData["anwser_right"];
				anwser_wrong = itemData["anwser_wrong"];
				click = itemData["click"];
				explain_show = itemData["explain_show"];
				next_level = itemData["next_level"];
				upgrade = itemData["upgrade"];
				sfx_common_btn = itemData["sfx_common_btn"];
				sfx_common_panel_show = itemData["sfx_common_panel_show"];
				sfx_special_panel_show = itemData["sfx_special_panel_show"];
				sfx_common_panel_close = itemData["sfx_common_panel_close"];
				sfx_common_gift_show = itemData["sfx_common_gift_show"];
				sfx_common_gift_wait = itemData["sfx_common_gift_wait"];
				sfx_common_gift_open = itemData["sfx_common_gift_open"];
				sfx_common_starfly = itemData["sfx_common_starfly"];
				sfx_common_starbeused = itemData["sfx_common_starbeused"];
				sfx_common_taskover = itemData["sfx_common_taskover"];
				sfx_common_taskappear = itemData["sfx_common_taskappear"];
				sfx_newtask_open = itemData["sfx_newtask_open"];
				sfx_newtask_close = itemData["sfx_newtask_close"];
				sfx_word_show = itemData["sfx_word_show"];
				sfx_bubble = itemData["sfx_bubble"];
				sfx_room_repair = itemData["sfx_room_repair"];
				sfx_newday = itemData["sfx_newday"];
				sfx_photo = itemData["sfx_photo"];
				sfx_turntable01 = itemData["sfx_turntable01"];
				sfx_turntable02 = itemData["sfx_turntable02"];
				sfx_common_getcoins = itemData["sfx_common_getcoins"];
				BGM_daily = itemData["BGM_daily"];
				sfx_logo = itemData["sfx_logo"];
				sfx_btn_normal = itemData["sfx_btn_normal"];
				sfx_letter = itemData["sfx_letter"];
				sfx_longpress = itemData["sfx_longpress"];
				sfx_transition = itemData["sfx_transition"];
				BGM_home = itemData["BGM_home"];
				sfx_progressincrease = itemData["sfx_progressincrease"];
				sfx_entergame = itemData["sfx_entergame"];
				sfx_pageturn = itemData["sfx_pageturn"];
				BGM_play1 = itemData["BGM_play1"];
				sfx_combo_good = itemData["sfx_combo_good"];
				sfx_combo_good_voice = itemData["sfx_combo_good_voice"];
				sfx_combo_great = itemData["sfx_combo_great"];
				sfx_combo_great_voice = itemData["sfx_combo_great_voice"];
				sfx_combo_excellent = itemData["sfx_combo_excellent"];
				sfx_combo_excellent_voice = itemData["sfx_combo_excellent_voice"];
				sfx_combo_amazing = itemData["sfx_combo_amazing"];
				sfx_combo_amazing_voice = itemData["sfx_combo_amazing_voice"];
				sfx_combo_outstanding = itemData["sfx_combo_outstanding"];
				sfx_combo_outstanding_voice = itemData["sfx_combo_outstanding_voice"];
				sfx_combo_unbelivable = itemData["sfx_combo_unbelivable"];
				sfx_combo_unbelivable_voice = itemData["sfx_combo_unbelivable_voice"];
				sfx_score = itemData["sfx_score"];
				sfx_click_letter_1 = itemData["sfx_click_letter_1"];
				sfx_click_letter_2 = itemData["sfx_click_letter_2"];
				sfx_click_letter_3 = itemData["sfx_click_letter_3"];
				sfx_click_letter_4 = itemData["sfx_click_letter_4"];
				sfx_click_letter_5 = itemData["sfx_click_letter_5"];
				sfx_click_letter_6 = itemData["sfx_click_letter_6"];
				sfx_click_letter_7 = itemData["sfx_click_letter_7"];
				sfx_shuffle = itemData["sfx_shuffle"];
				sfx_coinword = itemData["sfx_coinword"];
				sfx_match = itemData["sfx_match"];
				sfx_wrong = itemData["sfx_wrong"];
				sfx_repeat = itemData["sfx_repeat"];
				sfx_bouns_win = itemData["sfx_bouns_win"];
				sfx_bouns_repeat = itemData["sfx_bouns_repeat"];
				sfx_addScore = itemData["sfx_addScore"];
				sfx_lastword = itemData["sfx_lastword"];
				sfx_subworld_open = itemData["sfx_subworld_open"];
				sfx_usehint = itemData["sfx_usehint"];
				sfx_specific_hint_click = itemData["sfx_specific_hint_click"];
				sfx_specific_hint = itemData["sfx_specific_hint"];
				sfx_mult_hint = itemData["sfx_mult_hint"];
				sfx_extra_collect = itemData["sfx_extra_collect"];
				sfx_level_win = itemData["sfx_level_win"];
				sfx_level_wait = itemData["sfx_level_wait"];
				sfx_level_getstar = itemData["sfx_level_getstar"];
				sfx_win_effect = itemData["sfx_win_effect"];
				sfx_win_stop = itemData["sfx_win_stop"];
				sfx_starfall1 = itemData["sfx_starfall1"];
				BGM_play_daily = itemData["BGM_play_daily"];
				sfx_dailyreward_flower = itemData["sfx_dailyreward_flower"];
				sfx_dailyreward_gift = itemData["sfx_dailyreward_gift"];
				sfx_daily_star_unlock = itemData["sfx_daily_star_unlock"];
				sfx_dailyreward_open = itemData["sfx_dailyreward_open"];
				sfx_complete_change = itemData["sfx_complete_change"];
				BGM_play2 = itemData["BGM_play2"];
				BGM_play3 = itemData["BGM_play3"];
				BGM_play4 = itemData["BGM_play4"];
				sfx_common_star = itemData["sfx_common_star"];

                Inited = true;
            }
        }

        public void Init (string tableStr) {


                JSONNode itemData = JSONNode.Parse (tableStr);
				anwser_right = itemData["anwser_right"];
				anwser_wrong = itemData["anwser_wrong"];
				click = itemData["click"];
				explain_show = itemData["explain_show"];
				next_level = itemData["next_level"];
				upgrade = itemData["upgrade"];
				sfx_common_btn = itemData["sfx_common_btn"];
				sfx_common_panel_show = itemData["sfx_common_panel_show"];
				sfx_special_panel_show = itemData["sfx_special_panel_show"];
				sfx_common_panel_close = itemData["sfx_common_panel_close"];
				sfx_common_gift_show = itemData["sfx_common_gift_show"];
				sfx_common_gift_wait = itemData["sfx_common_gift_wait"];
				sfx_common_gift_open = itemData["sfx_common_gift_open"];
				sfx_common_starfly = itemData["sfx_common_starfly"];
				sfx_common_starbeused = itemData["sfx_common_starbeused"];
				sfx_common_taskover = itemData["sfx_common_taskover"];
				sfx_common_taskappear = itemData["sfx_common_taskappear"];
				sfx_newtask_open = itemData["sfx_newtask_open"];
				sfx_newtask_close = itemData["sfx_newtask_close"];
				sfx_word_show = itemData["sfx_word_show"];
				sfx_bubble = itemData["sfx_bubble"];
				sfx_room_repair = itemData["sfx_room_repair"];
				sfx_newday = itemData["sfx_newday"];
				sfx_photo = itemData["sfx_photo"];
				sfx_turntable01 = itemData["sfx_turntable01"];
				sfx_turntable02 = itemData["sfx_turntable02"];
				sfx_common_getcoins = itemData["sfx_common_getcoins"];
				BGM_daily = itemData["BGM_daily"];
				sfx_logo = itemData["sfx_logo"];
				sfx_btn_normal = itemData["sfx_btn_normal"];
				sfx_letter = itemData["sfx_letter"];
				sfx_longpress = itemData["sfx_longpress"];
				sfx_transition = itemData["sfx_transition"];
				BGM_home = itemData["BGM_home"];
				sfx_progressincrease = itemData["sfx_progressincrease"];
				sfx_entergame = itemData["sfx_entergame"];
				sfx_pageturn = itemData["sfx_pageturn"];
				BGM_play1 = itemData["BGM_play1"];
				sfx_combo_good = itemData["sfx_combo_good"];
				sfx_combo_good_voice = itemData["sfx_combo_good_voice"];
				sfx_combo_great = itemData["sfx_combo_great"];
				sfx_combo_great_voice = itemData["sfx_combo_great_voice"];
				sfx_combo_excellent = itemData["sfx_combo_excellent"];
				sfx_combo_excellent_voice = itemData["sfx_combo_excellent_voice"];
				sfx_combo_amazing = itemData["sfx_combo_amazing"];
				sfx_combo_amazing_voice = itemData["sfx_combo_amazing_voice"];
				sfx_combo_outstanding = itemData["sfx_combo_outstanding"];
				sfx_combo_outstanding_voice = itemData["sfx_combo_outstanding_voice"];
				sfx_combo_unbelivable = itemData["sfx_combo_unbelivable"];
				sfx_combo_unbelivable_voice = itemData["sfx_combo_unbelivable_voice"];
				sfx_score = itemData["sfx_score"];
				sfx_click_letter_1 = itemData["sfx_click_letter_1"];
				sfx_click_letter_2 = itemData["sfx_click_letter_2"];
				sfx_click_letter_3 = itemData["sfx_click_letter_3"];
				sfx_click_letter_4 = itemData["sfx_click_letter_4"];
				sfx_click_letter_5 = itemData["sfx_click_letter_5"];
				sfx_click_letter_6 = itemData["sfx_click_letter_6"];
				sfx_click_letter_7 = itemData["sfx_click_letter_7"];
				sfx_shuffle = itemData["sfx_shuffle"];
				sfx_coinword = itemData["sfx_coinword"];
				sfx_match = itemData["sfx_match"];
				sfx_wrong = itemData["sfx_wrong"];
				sfx_repeat = itemData["sfx_repeat"];
				sfx_bouns_win = itemData["sfx_bouns_win"];
				sfx_bouns_repeat = itemData["sfx_bouns_repeat"];
				sfx_addScore = itemData["sfx_addScore"];
				sfx_lastword = itemData["sfx_lastword"];
				sfx_subworld_open = itemData["sfx_subworld_open"];
				sfx_usehint = itemData["sfx_usehint"];
				sfx_specific_hint_click = itemData["sfx_specific_hint_click"];
				sfx_specific_hint = itemData["sfx_specific_hint"];
				sfx_mult_hint = itemData["sfx_mult_hint"];
				sfx_extra_collect = itemData["sfx_extra_collect"];
				sfx_level_win = itemData["sfx_level_win"];
				sfx_level_wait = itemData["sfx_level_wait"];
				sfx_level_getstar = itemData["sfx_level_getstar"];
				sfx_win_effect = itemData["sfx_win_effect"];
				sfx_win_stop = itemData["sfx_win_stop"];
				sfx_starfall1 = itemData["sfx_starfall1"];
				BGM_play_daily = itemData["BGM_play_daily"];
				sfx_dailyreward_flower = itemData["sfx_dailyreward_flower"];
				sfx_dailyreward_gift = itemData["sfx_dailyreward_gift"];
				sfx_daily_star_unlock = itemData["sfx_daily_star_unlock"];
				sfx_dailyreward_open = itemData["sfx_dailyreward_open"];
				sfx_complete_change = itemData["sfx_complete_change"];
				BGM_play2 = itemData["BGM_play2"];
				BGM_play3 = itemData["BGM_play3"];
				BGM_play4 = itemData["BGM_play4"];
				sfx_common_star = itemData["sfx_common_star"];

                Inited = true;
        }


    }
}