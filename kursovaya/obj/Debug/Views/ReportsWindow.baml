   M S B A M L   `   `   `  ����D  @kursovaya, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:kursovaya.Viewskursovaya.Views  5   	   #    kursovaya.Views.ReportsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:kursovaya.Views     5   	     � Title$  Отчеты��5   	   $	��450��6   $	��800��6%   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��10q�6"   .+�5      �� $��Период:��6   $��0,0,0,5q�6'   �� 5       �� Orientation$ 
Horizontal=�6   $��0,0,0,10q�62   .+�5      (  "System.Windows.Controls.DatePicker  $	��120��6@   $��0,0,10,0q�6L      SelectedDate 6   �� *	StartDate+6L   �� 5      $
��по��6   $��0,5,10,0q�6&     5      $	��120��6>    6   �� *	EndDate+6>   5      �� 5      $)��#Сформировать отчет��6   $	��150��5      $
��Left=�5      ��5      �� *GenerateReportCommand+5      5      &   System.Windows.Controls.DataGrid  5   
   $��1��6   $��10q�5         AutoGenerateColumns .    
IsReadOnly . ��5      �� *
ReportData+5!      � 5#   
   $��2��6   $��10q�6    � ColumnDefinitions 5$      �� 5%      $��* �6#   �� 5&      $
��Auto �6#   .+�5)      �� $��0��6   $ 
Horizontal=�6)   .+�5*      �� $$��Путь сохранения:��6   $��Center=�64   $��0,0,10,0q�6O   �� 5+      $	��300��5,      $��0,0,10,0q�5-      ��5+      �� *
SavePath+ �� UpdateSourceTrigger$ PropertyChanged=�5-      �� 5.      $��
Обзор��6   $��80��50      ��5/      �� *BrowseCommand+50      51      �� 53      $��1��6   $��Экспорт в PDF��54      $	��120��56      ��55      �� *ExportToPdfCommand+56      57      58      59      