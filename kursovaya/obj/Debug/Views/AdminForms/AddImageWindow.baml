   M S B A M L   `   `   `  ����  	kursovaya$    kursovaya.Views.AddImageWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   $	��302��5   	   $	��422��6     � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title 5   	   �� *WindowTitle+.��5      � $��10q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $)��#Путь к изображению:��6   $��5q�6/   �� 5   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��5q�6;   .+�5      �� $	��250��6V   $��0,0,5,0q�6b   ��6   �� *	ImagePath+ �� UpdateSourceTrigger$ PropertyChanged=�6b   �� 5      $��
Обзор��6   $��75��6G   ��6%   �� *BrowseCommand+6G   5      �� 5   
   $��2��6   $/��)ID тура (необязательно):��6!   $��5q�6A   �� 5   
   $��3��6   $��	5,5,5,133q�6\    J� RenderTransformOrigin$ 0.492,1.272&�6o   ��6   �� *TourId+$ PropertyChanged=�6o   �� 5   
   $��3��6   $ 
Horizontal=�6"   $��Right=�6;   $��0,87,0,0q�6W   $	��224��6i   .+�5      �� $��Сохранить��6   $	��100��6I   $��30��6U   $��5q�6a   ��6)   �� *SaveCommand+6a   �� 5      -   5        $��Отмена��5      $	��100��6@   $��30��6L   $��5q�6X   5      5      5      