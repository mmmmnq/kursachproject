   M S B A M L   `   `   `  ����D  @kursovaya, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:kursovaya.ViewModelskursovaya.ViewModels  5   	   5clr-namespace:kursovaya.Helperskursovaya.Helpers  5   	   (    "kursovaya.Views.ManageImagesWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1vm"clr-namespace:kursovaya.ViewModels     5   	   3helpersclr-namespace:kursovaya.Helpers     5   	     � Title$5  /Управление изображениями��5   	   $	��700��5   	   $
��1000��6   ���#121212 � WindowStartupLocation$ CenterScreen=�5	   	    � WindowStyle$
 None=�5
   	    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency .  � 	Resources 5         AccentButtonStyle �� 
TargetType �� Property �� Value	 �� 
TargetType
 �� CornerRadius �� Padding �� Triggers P� Property P� Value&   System.Windows.Controls.DataGrid=  7System.Windows.Controls.Primitives.DataGridColumnHeader��%�  &	        '  &    '  �    �� 5   
   " ��6*   .�5      ��  � �� �#007ACC6+   �� 5       � ��
 �����6+   �� 5       � ��$
 15,8q�6(   �� 5       � �� 5      �� 5      "	 ��6&   .R�5      �� #��z��6!   $
 8��5   !   # z��5   !   ._�5      �� $��Center=�6/   $��Center=�5   -   5       5      P� 5       �@J�IsMouseOver . . �5   "   ��  � �� �#0059996?   5       P� 5!       � {� .  . �5"   "   ��  � �� �#4D4D4D6?   5#      5%      5'      5(      �� 5+   
   "  6   .�5,      ��  � ��
 ���� 6+   �� 5-       � ��$ 1q�60   �� 5.       � �� �#3333336,   �� 5/       �@ RowBackground �#1E1E1E6.   �� 50        �@ AlternatingRowBackground �#25252569   �� 51       �@ GridLinesVisibility$
 None=�64   �� 52       � ��
 �����6+   53      �� 55   
   "  6   .�56      ��  � �� �#2525256+   �� 57       � ��
 �����6+   �� 58       � ��$
 10,5q�6(   �� 59       � ��$ 0,0,1,1q�60   �� 5:       � �� �#3333336,   5;      5<      .��5>      �� 
������ ���#333333$��1q�5@      $
 20��5A      $��10q�5B      ._�5C   
   �  � RowDefinitions 5D      �� 5E      $
��Auto �6    �� 5F      $��* �6    �� 5G      $
��Auto �6    .+�5K      �� $��0��6   ���#1E1E1E$
 	20,20,0,0��5M      $ 20q�5N      ._�5O      � .+�5P      �� $5��/Управление изображениями��6    $��32(�5Q      $��SemiBold"�5R      
�������$��Center=�5T      �� 5U      -   5        $	��✕��5U      $��Right=�5V      #��[  5W      $��32��5Y      $��32��5Z      $��0q�5[      $	��Top=�5\      5]      5^      � 5a      $��1��6   $��20q�6     � ColumnDefinitions 5b      �� 5c      $��* �6'   �� 5d      $
��Auto �6'   .+�5h           AutoGenerateColumns .     
IsReadOnly .    SelectionMode$ Single=�5l         SelectionUnit$ FullRow=�5m         CanUserResizeRows .     CanUserAddRows .  $��0,0,20,0q�5p      ��5h      �� *Images+ �� SelectedItem 5i      �� *SelectedImage+ �� Mode$ TwoWay=�   Columns 5q      0  *System.Windows.Controls.DataGridTextColumn  5r      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6U   1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *	ImageId+6U     5s      $ ID тура��6-   $
 Auto 6Y    6>   �� *TourId+6Y     5t      $ Путь к файлу��6-   $ * 6a    6C   �� *	ImagePath+6a   5v      �� 5y      $��1��6   $	��300��5z      ���#252525$
 8��5|      $ 10q�5}      ._�5~      �� $��Uniform=�5      $	��300��5�      ��5~      �� *SelectedImage.ImagePath+5�      5�      5�      �� 5�       �� Orientation$ 
Horizontal=�6   $��2��5�      $��Center=�5�      $��10q�5�      .+�5�      �� $-��'Добавить изображение��6   $��10q�5�      #��[  5�      ��5�      �� *OpenAddImageWindowCommand+5�      �� 5�      $��Изменить��6   $��10q�5�      #��[  5�      ��5�      �� *EditImageCommand+{�5�      �� *SelectedImage+  NullToBoolConverter �� 	Converter# [ 5�      �� 5�      $+��%Удалить изображение��6   $��10q�5�      #��[  5�      ��5�      �� *DeleteImageCommand+{�5�      �� *SelectedImage+5�      5�      5�      5�      5�      