   M S B A M L   `   `   `  ����E  ATaskMaster, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:TaskMaster
TaskMaster  5   	       TaskMaster.admin   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:TaskMaster     5   	     � Title$(  "Админестрирование��5   	   $	��700��6#   $
��1220��60    � WindowStartupLocation$ CenterScreen=�6=    � Icon$ /icons8-тм-48.png��6b    � 	Resources 5
         ListBoxItemStyle �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers	 P� Property
 P� Value �� 
TargetName��%�  &	        �� 5   
   " ��6)   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� $��border��6!   #��z��6/   #��z��6Y   #��z��6�   ._�5      �� $��Stretch=�6/   $��Stretch=�6M   5       5      P� 5      	 �@J�IsMouseOver
 . . �5   "   �� $ border��6)    � ��
 �  ��6T   5      P� 5      	 � {�
 .  . �5   "   �� # Z�6?    � ���� 5   "   # Z
�6?    � ��5      5      5      5      5!      .��5#      � -   5         �Name$
 mane��5#       � ColumnDefinitions 5%   
   �� 5&      $��0.35* �6   �� 5'      .+�5+   
   � -   5        $
 menu��5+      ���	#FFB1ACAC$��0��64    � RowDefinitions 5,      �� 5-      $��50 �6    �� 5.      �� 5/      $	��100 �6    .+�52      � -   5        $ 	butonmenu��52      $��1��6&    53      �� 54      �� 55      �� 56      .+�58      �� -   5        $��taskbt��58        AdminButtonStyle#��[ 6'   $��0��6Q   $��Задачи��6^   $��Bottom=�6�   �� 59      -   5        $��userbt��59      #��[ 6'   $��1��6Q   $��Сотрудники��6^   6s   �� 5:      -   5        $��adminbt��5:      #��[ 6(   $��2��6R   $"��Администраторы��6_   $	��Top=�6�   5<      �� 5=      -   5        $��exitakk��5=        CustomButtonStyle#��[ 6$   $��2��6O   $&�� Выйти из аккаунта��6\   6x   5>      � 5@   
   -   5        $ 	manechast��5@      $��1��6"    5A      �� 5B      $��45 �6    �� 5C      .+�5F      � -	   5        $ taskbar��5F      $��0��6$   ���	#FFB1ACAC 5G      �� 5H      .+�5K      ��  �� Orientation$ 
Horizontal=�6   $
��Left=�66   .+�5L      �� -
   5        $��userCb��5L      $	��170��6-   $��	10,5,15,5q�69   $��Bahnschrift SemiBold)�6L   $��20(�6n   �� 5M      -   5        $��typeCb��5M      $	��170��6-   $��0,5,15,5q�69   $��Bahnschrift SemiBold)�6K   $��20(�6m   �� 5N      -   5        $��statusCb��5N      $	��170��6/   $��0,5,10,5q�6;   $��Bahnschrift SemiBold)�6M   $��20(�6o   �� 5O      -   5         ��Name$ serchbt��5O        ImgButtonStyle#��[ 6.   $��Right=�6V   $��1��5P      $��Center=�6+   ��5Q      �� 5R       �� ImageSource$ 
/serch.png��6)   5T      �� 5V      -   5        $ resetFilterbt��5V      $��1��64   #��[ 6D   ��5W      �� 5X      $% /icons8-обновить-48.png��6)   5Z      5[      5]      �� 5_      -   5        $��TaskList��5_      $y�Visible=�6&   $��1��6;   
�������$��Hidden=�5`      
�������$��Stretch=�6K   ��5a      �� 5b      " ��6    �� BasedOn# [  65   ��5d      �� 5e      .R�5f      �� 
���   �$��1q�65    �� Padding$ 10, 10, 10, 10q�6I   $��Stretch=�6c   ._�5g      � $	��170��6#   $��0q�65   $	��810��6@    5h   "   �� 5i   &   $��30 �64   �� 5j   &   $��50 �64   �� 5k   &   $	��110 �65   �� 5l   &   .+�5o   "   ��  ��Name$
 user��6,   $��0��6:   $
��Wrap=�6G   $��Bookman Old Style)�6[   $��24(�5p   ,   
���   �$��Bottom=�6M   $
��Bold"�6h   �� 5q   -    � Language$ ru-ru�61   �6B   �� *
fullName+5r   #   � 5t   "   $��1��6'    5u   &   �� 5v   *   �� 5w   *   �� 5x   *   .+�5|   &   �� $
 type��60   $��0��6>   $��Bahnschrift SemiBold)�6N   $��26(�6p   $��Center=�6~   $
��Left=�6�   �� 5}   ,   $ ru-ru�60   �6A   �� *	id_type+6]   �� 5~   &   $ location��60   $��1��6B    �� TextAlignment$ Center=�6R   $��Bahnschrift SemiBold)�6i   $��24(�6�   $��Center=�6�   $
��Wrap=�6�   $��Center=�6�   �� 5   ,   $ ru-ru�60   �6A   �� *
location+6^   �� 5�   &   $
 sroc��60   $��2��6>   $
��Wrap=�6N   $��Bahnschrift SemiBold)�6b   $��26(�6�   $��Right=�5�   1   $��Center=�6M     OverdueTaskStyle#��[ 6h   �� 5�   *   $ ru-ru�6.   �6?   �� *sroc+5�   '   5�   #   �� 5�   "   $
 text��6,   $��2��6:   $
��Wrap=�6G   $��Bahnschrift SemiBold)�6[   $��24(�6}   ���	#FF4A4444�� 5�      $ ru-ru�6   �6,   �� *text+6E   � 5�   "   $��3��6'    5�   &   �� 5�   *   �� 5�   *   .+�5�   &   � $��0��6+    5�   *   �� 5�   .   �� 5�   .   �� 5�   .   .+�5�   *   �� $��0��65   $ 
Horizontal=�6B   $
��Left=�6[   .+�5�   .   �� $��0��68   $
��Wrap=�6H   $��Bahnschrift SemiBold)�6\   $��26(�6~   ���	#FF4A4444$
��Left=�6�   �� 5�   7   $ ru-ru�6;   �6L   �� *status+6g   5�   +   �� 5�   *   $ resbt��61   -   5        $��1��5�   @   $��17(�6P   $��Center=�6_   $	��130��6|   $��30��6�   $��0q�6�   #��[ 6�   $��Yu Gothic UI Semibold)�5�   /   $��0q�6R   
���������5�   .   � 5�   2    5�   6   �� 5�   :   $
��Auto �6K   �� 5�   :   $
��Auto �6K   .+�5�   6   �� $��РЕЗУЛЬТАТ��6@   $��Center=�6Q   $��0��6l   �� 5�   6   $;��5/icons8-двойная-стрелка-вниз-30.png��6<   $��1��6i   $��5,0,0,0q�6z   $��Center=�6�   $��22��6�   5�   3   5�   +   �� 5�   *   $
 time��64   $��2��6B   $
��Wrap=�6R   $��0q�6f   $��Right=�6r   $��Bahnschrift SemiBold)�6�   $��23(�6�   ���	#FF777272$��Center=�6�   �� 5�   .   $ ru-ru�62   �6C   �� *time+6\   5�   '   � 5�   &   $��resgrid��6+   $��1��6:   $y�	Collapsed=�6G    5�   *   �� 5�   .   $��2* �6@   �� 5�   .   .+�5�   *   �� $��0��62   $
��0, 7q�6B   $	��1.5q�6P   ���	#FF121010$��18(�6~    �� TextWrapping$
 Wrap=�6�   ���#ED1C24$
��Bold"�6�   ��6�   �� *	comment+6�   � 5�   *   $��1��6/   $��Right=�6?    5�   .   �� 5�   2   �� 5�   2   .+�5�   .   �� $��0��69   $ 
Horizontal=�6F   $��Center=�6_   .+�5�   2   �� $ imgbt��69   $��2��6I   #��[ 6V   $��0,0,7,0q�5�   :   ��5�   6   �� 5�   :   $# /icons8-скрепка-48.png��6E   5�   3   �� 5�   2    ��Name$ filetb��6:   $��18(�6J   $��3��6X   ���	#FF121010$��1��6�   $��Center=�6�   ���#ED1C24$ NoWrap=�5�   5   $��Center=�6K   $	��200��6f   $��27��6�   $
��Bold"�6�   ��5�   �   �� *
fileResult+��5�   r   Z� 6�   5�   /   �� 5�   .   $��1��69   $ 
Horizontal=�6F   $��Center=�6_   .+�5�   2   �� $ imgebt��69   $��2��6J   $��38��6W   $��39��6b   #��[ 6n   $��0,0,7,0q�6�   ��5�   6   �� 5�   :   $! /icons8-фото-32 (1).png��6E   5�   3   �� 5�   2   $ phototb��6:   $��18(�6K   ���	#FF121010$��3��6�   $��1��6�   $��Center=�6�   ���#ED1C24$ NoWrap=�5�   9   $��Center=�6O   $	��200��6j   $��27��6�   $
��Bold"�6�   ��5�   p   �� *photoResult+��5�   v   Z� 6�   5�   /   5�   +   5�   '   5�   #   5�      5�      5�       5�        MainScrollViewer��%0   '��       7  �� 5�      " ��6   8   66   5�      5�      5�      5�      5�      