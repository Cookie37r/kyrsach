   M S B A M L   `   `   `  ����E  ATaskMaster, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:TaskMaster
TaskMaster  5   	       TaskMaster.AdminAdd   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:TaskMaster     5   	     � Title$?  9Редактирование администратора��5   	   ���#ED1C24$	��600��6E   $	��650��6R    � Icon$ /icons8-тм-48.png��6^    � WindowStartupLocation$ CenterScreen=�6w   .��5	      � 
������� � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      .+�5   
   � -   5         �Name$ BD��5       � ColumnDefinitions 5      �� 5      $��2* �6$   .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ dataGridAdmin��5      $��1��6.   $��5q�6>      AutoGenerateColumns .  $��16(�6e      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 ID��6)   
   
Visibility$
 	Collapsed=�6Q   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *	ID_user+6Q     5      $	 
Логин��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6R    68   �� *Login+6R     5      $	 Пароль��6)   $ * 6V    69   �� *
Password+6V   5      5      � 5   
   -   5        $ toolbar��5      $��1��6     5       �� 5!      $
��1.5* �6#   �� 5"      .+�5%      �� $��1��6   $��8q�6(   
���   � �� RadiusX$ 15��5&       �� RadiusY$ 15��6"   ���	#FF373030 J� Opacity$
 0.25Q�6@   �� 5(      $��0��6   $��8q�6(   
���   �$ 15��5)      $ 15��6"   ���	#FF373030$
 0.25Q�6@   � 5+      $��9q�6    5,      �� 5-      �� 5.       51      �� 52      $	��150 �6'   �� 53      .+�56      �� $��Center=�6   $��Bahnschrift SemiBold)�57      $��24(�58      ���	#FF4A4444$
��Wrap=�5:      $��Логин:��6%   $��Center=�63   �� 5<      $��Center=�6   $��Bahnschrift SemiBold)�5=      $��24(�5>      ���	#FF4A4444$
��Wrap=�5@      $��Пароль:��6%   $��Center=�64   $��1��6O   �� 5B      -   5         ��Name$ logintb��5B      $��1��6+   $
��Left=�6;   ���#ED1C24$��Bahnschrift SemiBold)�6m   $��18(�6�    �� TextWrapping$ NoWrap=�5C      $��Center=�6,   $	��185��6G   
�������$��27��6f   �� 5E      -   5        $ 
passwordtb��5E      $��1��6.   $��Bahnschrift SemiBold)�6?   $��18(�6a   $��1��6o   $
��Left=�6|   ���#ED1C24$ NoWrap=�5F      $��Center=�6,   $	��185��6G   
�������$��27��6f   5J      � 5L      -   5        $ bt��5L      $��1��6    5N      �� 5O      �� 5P      �� 5Q      .+�5T      �� -   5         ��Name$	 add��5T   9   $��0��6   $��Добавить��6&   $	��150��6F   $��21(�6R   $��Dubai Medium)�5U      $��0, 10, 0, 0q�63      CustomButtonStyle#��[  6H   5V      �� 5X      -   5        $ delete��5X   8   $��1��6   $��Удалить��6&   $	��150��6H   $��21(�6T   $��Dubai Medium)�5Y      $��0, 10, 0, 0q�63   #��[  6H   5Z      �� 5\      -	   5        $
 save��5\   :   $��2��6   $��Сохранить��6&   $	��150��6I   $��21(�6U   $��Dubai Medium)�5]      $��0, 10, 0, 0q�63   #��[  6H   5^      5`      5a      5c      5e      