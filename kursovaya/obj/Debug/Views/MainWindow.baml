   M S B A M L   `   `   `  ����D  @kursovaya, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:kursovaya.ViewModelskursovaya.ViewModels  5   	        kursovaya.Views.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1vm"clr-namespace:kursovaya.ViewModels     5   	     � Title$  Авторизация��5   	   $	��450��6   $	��800��6*    � DataContext 5      )   #kursovaya.ViewModels.LoginViewModel  5	   
   .��5      � ���#1e1e1e$��Center=�6    $��Center=�6=   $	��450��6X   $	��800��6e    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   � $��3��6   $	��300��6   $��35��6(   $	��Top=�64   $��245,139,255,0q�6L   .+�5      �� -   5         ��Name$ usernameTextBox��5      ���#333333���#ffffff$��16(�5      $��5q�6$   ��5      �� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=�5   $   �� 5      $��
Логин��6   ���#777777$��16(�5      $��Center=�5      $
��10,0q�5       J� IsHitTestVisible .  y�5       �� *Text+ �� ElementName$ usernameTextBox��   StringToVisibilityConverter �� 	Converter# [  5!      � 5$   
   $��3��6   $	��300��6   $��35��6(   $	��Top=�64   $��245,174,255,0q�6L   .+�5%      G� -   5         G�Name$ passwordTextBox��5%      ���#333333���#ffffff$��16(�5'      $��5q�6(   5(      �� 5)      $��Пароль��6   ���#777777$��16(�5+      $��Center=�5,      $
��10,0q�5-       .  y�5/      �� *
Password+$ passwordTextBox��# [  50      �� 51   
   -   5        $6��0Еще нет аккаунта? Создайте��51      $
��Left=�66   $	��Top=�6Q   $��306,359,0,0q�6i   $��3��6�   �� 54   
   $��3��6   $��
Войти��6   $	��150��6.   $��40��6:   ���#007ACC���#ffffff$
��Left=�57      $��310,303,0,89q�6,   ��55      �� *LoginCommand+57   ,   58      59      